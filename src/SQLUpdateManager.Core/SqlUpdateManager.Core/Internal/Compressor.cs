using System;
using System.Collections.Generic;
using System.Text;

namespace SqlUpdateManager.Core
{
    internal static class Compressor
    {
		internal static byte[] Compress(string data)
        {
			var tree = new Coder.Tree();

			tree.BuildDictionary(data);

            var coder = new Coder(tree);

			return coder.Encode(Encoding.UTF8.GetBytes(data));
        }

		internal static byte[] Decompress(string data)
        {
			var tree = new Coder.Tree();

			tree.BuildDictionary(data);

            var coder = new Coder(tree);

            coder.Decode(Encoding.UTF8.GetBytes(data), out var result);

            return result;
        }
    }

	internal class Coder
    {
		internal Coder(Tree tree)
        {
            _Tree = tree;
        }

        private Tree _Tree = null;

		internal uint Encode(byte[] input, out byte[] output)
        {
            var ret = new List<byte>();

            unchecked
            {
                int carryOverBits = 0,
					totalBytes,
					currentBits,
					bufferIndex = -1,
					adjust;
                byte byteValue;
                bool shiftDirection;
                Tree.ForwardLookupEntry entry;

                foreach (var b in input)
                {
                    entry = _Tree.ForwardLookup[b];
                    currentBits = entry.CurrentBits;
                    totalBytes = entry.TotalBytes;

                    do
                    {
                        if (currentBits > 0)
                        {
                            byteValue = (byte)(entry.Value >> totalBytes);

                            shiftDirection = currentBits > carryOverBits;
                            adjust = shiftDirection ? currentBits - carryOverBits : carryOverBits - currentBits;

                            if (carryOverBits > 0)
                            {
                                if (shiftDirection)
                                    ret[bufferIndex] = (byte)(ret[bufferIndex] | ((byteValue & (0xFF >> (8 - currentBits))) >> adjust));

                                else
                                    ret[bufferIndex] = (byte)(ret[bufferIndex] | ((byteValue & (0xFF >> (8 - currentBits))) << adjust));
                            }

                            if (shiftDirection)
                            {
                                ret.Add((byte)(byteValue << (carryOverBits = 8 - adjust)));
                                bufferIndex++;
                            }

                            else
                                carryOverBits = adjust;
                        }

                        if (totalBytes > 0)
                            currentBits = 8;
                    }
                    while ((totalBytes -= 8) >= 0);
                }

                if (_Tree.Padding)
                    ret[bufferIndex] = (byte)(ret[bufferIndex] | (0xFF >> (8 - carryOverBits)));

                output = ret.ToArray();

                return (uint)((bufferIndex << 3) + (8 - carryOverBits));
            }
        }

		internal uint Encode(string input, out byte[] output) =>
            Encode(Encoding.UTF8.GetBytes(input), out output);

		internal byte[] Encode(byte[] buffer)
        {
            Encode(buffer, out var result);

            return result;
        }

		internal byte[] Encode(string buffer) =>
            Encode(Encoding.UTF8.GetBytes(buffer));

		internal uint Decode(byte[] input, out byte[] output, int inputOffset = 0, uint bitlength = 0)
        {
            var ret = new List<byte>();

            unchecked
            {
                uint RemainingBufferLength = (uint)(input.Length - inputOffset) << 3,
                    initialBufferLength = RemainingBufferLength;

                if (bitlength > 0 && bitlength < RemainingBufferLength)
                    RemainingBufferLength = bitlength;

				var bufferIndex = inputOffset;
				var carryOverBitlen = 0;
				byte carryOverByte = 0;

                bool keepSearching = false,
                    processSearch = false;
                byte bestMatchIndex = 0;
                byte bestMatchValue = 0;

                for (byte i = _Tree.MinBitLen; ; i++)
                {
                    if (i > _Tree.MaxBitLen)
                    {
                        if (keepSearching)
                            processSearch = true;

                        else
                            break;
                    }

                    else if (i <= RemainingBufferLength)
                    {
                        if (_Tree.ReverseLookup.ContainsKey(i))
                        {
                            int tmpBufferIndex = bufferIndex;
                            byte tmpCarryOverByte = carryOverByte;
                            int tmpCarryOverBitlen = carryOverBitlen;
                            int countOfBits = i;
                            uint valueIndex = 0;

                            while (countOfBits > 0)
                            {
                                int countToRead = 8;

                                if (countOfBits < countToRead)
                                    countToRead = countOfBits;

                                valueIndex <<= countToRead;

                                byte returnByte = 0;

                                if (tmpCarryOverBitlen >= countToRead)
                                {
                                    returnByte = (byte)(tmpCarryOverByte >> (8 - countToRead));
                                    tmpCarryOverByte <<= countToRead;
                                    tmpCarryOverBitlen -= countToRead;
                                }

                                else
                                {
                                    byte nextByte = input[tmpBufferIndex++];
                                    int offset = countToRead - tmpCarryOverBitlen;

                                    returnByte = (byte)((tmpCarryOverByte >> (8 - countToRead)) | (nextByte >> (Math.Abs(offset - 8))));
                                    tmpCarryOverByte = (byte)(nextByte << offset);
                                    tmpCarryOverBitlen = 8 - offset;
                                }

                                valueIndex |= returnByte;
                                countOfBits -= countToRead;
                            }

                            if (_Tree.ReverseLookup[i].ContainsKey(valueIndex))
                            {
                                var entry = _Tree.ReverseLookup[i][valueIndex];

                                bestMatchIndex = i;
                                bestMatchValue = entry.Value;

                                if (entry.Collides)
                                    keepSearching = true;

                                else
                                    processSearch = true;
                            }
                        }
                    }

                    else
                    {
                        if (keepSearching)
                            processSearch = true;

                        else
                            break;
                    }

                    if (processSearch)
                    {
                        int countOfBits = bestMatchIndex;

                        while (countOfBits > 0)
                        {
                            int countToRead = 8;

                            if (countOfBits < countToRead)
                                countToRead = countOfBits;

                            RemainingBufferLength -= (uint)countToRead;

                            if (carryOverBitlen >= countToRead)
                            {
                                carryOverByte <<= countToRead;
                                carryOverBitlen -= countToRead;
                            }

                            else
                            {
                                int offset = countToRead - carryOverBitlen;

                                carryOverByte = (byte)(input[bufferIndex++] << offset);
                                carryOverBitlen = 8 - offset;
                            }

                            countOfBits -= countToRead;
                        }

                        ret.Add(bestMatchValue);
                        i = (byte)(_Tree.MinBitLen - 1);
                        processSearch = keepSearching = false;
                    }
                }

                output = ret.ToArray();

                return initialBufferLength - RemainingBufferLength;
            }
        }

		internal class Tree
		{
			internal Tree()
			{
			}

			internal Tree(Dictionary<byte, Dictionary<uint, byte>> lookup)
			{
				Init(lookup);
			}

			internal bool Padding = false;

			internal Dictionary<byte, Dictionary<uint, ReverseLookupEntry>> ReverseLookup { private set; get; }

			internal ForwardLookupEntry[] ForwardLookup { private set; get; }

			internal byte MinBitLen { private set; get; }

			internal byte MaxBitLen { private set; get; }

			private void Init(Dictionary<byte, Dictionary<uint, byte>> lookup)
			{
				if (lookup == null)
				{
					throw new InvalidOperationException("No lookup table has been created. Either provide one or generate it");
				}

				MinBitLen = 32;
				MaxBitLen = 0;
				foreach (byte v in lookup.Keys)
				{
					if (v < MinBitLen)
					{
						MinBitLen = v;
					}
					if (v > MaxBitLen)
					{
						MaxBitLen = v;
					}
				}

				ForwardLookup = new ForwardLookupEntry[256];
				ReverseLookup = new Dictionary<byte, Dictionary<uint, ReverseLookupEntry>>();
				foreach (KeyValuePair<byte, Dictionary<uint, byte>> kv in lookup)
				{
					Dictionary<uint, ReverseLookupEntry> entry = new Dictionary<uint, ReverseLookupEntry>();
					foreach (KeyValuePair<uint, byte> kkv in kv.Value)
					{
						ForwardLookup[kkv.Value] = new ForwardLookupEntry()
						{
							Value = kkv.Key,
							BitLength = kv.Key,
							CurrentBits = (byte)(kv.Key % 8),
							TotalBytes = (byte)(kv.Key >> 3 << 3)
						};
						entry.Add(kkv.Key, new ReverseLookupEntry()
						{
							Value = kkv.Value,
							Collides = false
						});
					}
					ReverseLookup.Add(kv.Key, entry);
				}

				Coder test = new Coder(this);
				HashSet<byte> clist = new HashSet<byte>();
				for (byte i = 0; i < 255; i++)
				{
					byte[] arr;
					test.Decode(test.Encode(new byte[1] { i }), out arr);
					if (arr.Length > 0)
					{
						byte t = arr[0];
						if (t != i)
						{
							if (!clist.Contains(t))
							{
								clist.Add(t);
							}

							if (!clist.Contains(i))
							{
								clist.Add(i);
							}
						}
					}
				}

				foreach (byte b in clist)
				{
					ForwardLookupEntry entry = ForwardLookup[b];
					ReverseLookup[(byte)entry.BitLength][entry.Value].Collides = true;
				}
			}

			internal void BuildDictionary(byte[] sample)
			{
				Dictionary<byte, uint> freq = new Dictionary<byte, uint>();
				foreach (byte b in sample)
				{
					if (freq.ContainsKey(b))
					{
						freq[b]++;
					}
					else
					{
						freq.Add(b, 0); // Start at 0, this value is relative
					}
				}

				List<KeyValuePair<byte, uint>> sortingList = new List<KeyValuePair<byte, uint>>(freq.Count);
				foreach (KeyValuePair<byte, uint> kv in freq)
				{
					sortingList.Add(kv);
				}

				sortingList.Sort(DictionaryListComparer.Instance);

				HashSet<uint> usedValues = new HashSet<uint>();
				Dictionary<byte, Dictionary<uint, byte>> lookup = new Dictionary<byte, Dictionary<uint, byte>>();

				byte currentbitlen;
				if (Math.Max(Math.Ceiling(Math.Log(freq.Count) / Math.Log(2)), 1) > 8)
				{
					currentbitlen = 5;
				}
				else
				{
					for (currentbitlen = 1; currentbitlen < 8 && freq.Count > ((1 << (currentbitlen + 1)) / 2) + (((1 << (currentbitlen + 1)) - ((1 << (currentbitlen + 1)) / 2)) / 2); currentbitlen++) ;
				}

				uint value = 0;
				sbyte shiftIndex = -1;
				byte shiftedBits = 1;
				bool shouldInvert = false;
				int possibleUniques = 0x1 << currentbitlen;
				int index = 0;
				unchecked
				{
					foreach (KeyValuePair<byte, uint> kv in sortingList)
					{
						for (; ; )
						{
							if (index == possibleUniques)
							{
								currentbitlen++;
								shiftIndex = -1;
								shiftedBits = 0;
								shouldInvert = false;
							}

							uint mask = (~((uint)0x0) >> (32 - currentbitlen));
							if (shouldInvert)
							{
								value = ~value & mask;
								shouldInvert = false;
								if (++shiftIndex == currentbitlen)
								{
									shiftedBits++;
									shiftIndex = 0;
								}
							}
							else
							{
								value = (0x0 | ~((uint)0x0) >> (32 - shiftedBits) << shiftIndex) & mask;
								shouldInvert = true;
							}

							index++;

							if (!usedValues.Contains(value))
							{
								usedValues.Add(value);
								if (lookup.ContainsKey(currentbitlen))
								{
									lookup[currentbitlen].Add(value, kv.Key);
								}
								else
								{
									lookup.Add(currentbitlen, new Dictionary<uint, byte>()
									{
										[value] = kv.Key
									});
								}
								break;
							}
						}
					}
				}

				Init(lookup);
			}

			internal void BuildDictionary(string sample)
			{
				BuildDictionary(Encoding.UTF8.GetBytes(sample));
			}

			internal class ReverseLookupEntry
			{
				internal byte Value;
				internal bool Collides;
			}

			internal struct ForwardLookupEntry
			{
				internal uint Value;
				internal byte BitLength;
				internal byte CurrentBits;
				internal byte TotalBytes;
			}

			private class DictionaryListComparer : Comparer<KeyValuePair<byte, uint>>
			{
				internal static DictionaryListComparer Instance = new DictionaryListComparer();

				public override int Compare(KeyValuePair<byte, uint> x, KeyValuePair<byte, uint> y)
				{
					return y.Value.CompareTo(x.Value);
				}
			}
		}
	}
}
