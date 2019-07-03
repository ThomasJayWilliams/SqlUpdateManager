using System;
using System.Collections.Generic;
using System.Text;

namespace SQLUpdateManager.Core.Internal
{
    public static class Compressor
    {
        public static string Compress(string data)
        {
            var coder = new Coder(new Coder.Tree());
            return Encoding.UTF8.GetString(coder.Encode(data));
        }

        public static string Decompress(string data)
        {
            var coder = new Coder(new Coder.Tree());
            coder.Decode(
                Encoding.UTF8.GetBytes(data),
                out var result);
            return Encoding.UTF8.GetString(result);
        }
    }

    public class Coder
    {
        public Coder(Tree tree)
        {
            _Tree = tree;
        }

        private Tree _Tree = null;

        public uint Encode(byte[] input, out byte[] output)
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

                foreach (byte b in input)
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

        public uint Encode(string input, out byte[] output) =>
            Encode(Encoding.UTF8.GetBytes(input), out output);

        public byte[] Encode(byte[] buffer)
        {
            Encode(buffer, out var result);
            return result;
        }

        public byte[] Encode(string buffer) =>
            Encode(Encoding.UTF8.GetBytes(buffer));

        public uint Decode(byte[] input, out byte[] output, int inputOffset = 0, uint bitlength = 0)
        {
            var ret = new List<byte>();

            unchecked
            {
                uint RemainingBufferLength = (uint)(input.Length - inputOffset) << 3,
                    initialBufferLength = RemainingBufferLength;

                if (bitlength > 0 && bitlength < RemainingBufferLength)
                {
                    RemainingBufferLength = bitlength;
                }

                int bufferIndex = inputOffset;
                byte carryOverByte = 0;
                int carryOverBitlen = 0;

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

        public class Tree
        {
            public Tree() { }

            public Tree(Dictionary<byte, Dictionary<uint, byte>> lookup)
            {
                Init(lookup);
            }

            public bool Padding = false;

            public Dictionary<byte, Dictionary<uint, ReverseLookupEntry>> ReverseLookup { private set; get; }

            public ForwardLookupEntry[] ForwardLookup { private set; get; }

            public byte MinBitLen { private set; get; }

            public byte MaxBitLen { private set; get; }

            private void Init(Dictionary<byte, Dictionary<uint, byte>> lookup)
            {
                if (lookup == null)
                    throw new InvalidOperationException("No lookup table has been created. Either provide one or generate it");

                MinBitLen = 32;
                MaxBitLen = 0;
                foreach (byte v in lookup.Keys)
                {
                    if (v < MinBitLen)
                        MinBitLen = v;
                    if (v > MaxBitLen)
                        MaxBitLen = v;
                }

                ForwardLookup = new ForwardLookupEntry[256];
                ReverseLookup = new Dictionary<byte, Dictionary<uint, ReverseLookupEntry>>();
                foreach (var kv in lookup)
                {
                    var entry = new Dictionary<uint, ReverseLookupEntry>();
                    foreach (var kkv in kv.Value)
                    {
                        ForwardLookup[kkv.Value] = new ForwardLookupEntry
                        {
                            Value = kkv.Key,
                            BitLength = kv.Key,
                            CurrentBits = (byte)(kv.Key % 8),
                            TotalBytes = (byte)(kv.Key >> 3 << 3)
                        };
                        entry.Add(kkv.Key, new ReverseLookupEntry
                        {
                            Value = kkv.Value,
                            Collides = false
                        });
                    }
                    ReverseLookup.Add(kv.Key, entry);
                }

                var test = new Coder(this);
                var clist = new HashSet<byte>();
                for (byte i = 0; i < 255; i++)
                {
                    test.Decode(test.Encode(new byte[1] { i }), out var arr);
                    if (arr.Length > 0)
                    {
                        byte t = arr[0];
                        if (t != i)
                        {
                            if (!clist.Contains(t))
                                clist.Add(t);
                            if (!clist.Contains(i))
                                clist.Add(i);
                        }
                    }
                }

                foreach (var b in clist)
                {
                    var entry = ForwardLookup[b];
                    ReverseLookup[entry.BitLength][entry.Value].Collides = true;
                }
            }

            public void BuildDictionary(byte[] sample)
            {
                var freq = new Dictionary<byte, uint>();
                foreach (var b in sample)
                {
                    if (freq.ContainsKey(b))
                        freq[b]++;
                    else
                        freq.Add(b, 0); // Start at 0, this value is relative
                }

                var sortingList = new List<KeyValuePair<byte, uint>>(freq.Count);
                foreach (var kv in freq)
                    sortingList.Add(kv);

                sortingList.Sort(DictionaryListComparer.Instance);

                var usedValues = new HashSet<uint>();
                var lookup = new Dictionary<byte, Dictionary<uint, byte>>();

                byte currentbitlen;
                if (Math.Max(Math.Ceiling(Math.Log(freq.Count) / Math.Log(2)), 1) > 8)
                    currentbitlen = 5;
                else
                    for (currentbitlen = 1;
                        currentbitlen < 8 && freq.Count > ((1 << (currentbitlen + 1)) / 2) + (((1 << (currentbitlen + 1)) - ((1 << (currentbitlen + 1)) / 2)) / 2);
                        currentbitlen++);

                uint value = 0;
                sbyte shiftIndex = -1;
                byte shiftedBits = 1;
                bool shouldInvert = false;
                int possibleUniques = 0x1 << currentbitlen;
                int index = 0;
                unchecked
                {
                    foreach (var kv in sortingList)
                    {
                        while (true)
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
                                    lookup[currentbitlen].Add(value, kv.Key);
                                else
                                {
                                    lookup.Add(currentbitlen, new Dictionary<uint, byte>
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

            public void BuildDictionary(string sample) =>
                BuildDictionary(Encoding.UTF8.GetBytes(sample));

            public class ReverseLookupEntry
            {
                public byte Value;
                public bool Collides;
            }

            public struct ForwardLookupEntry
            {
                public uint Value;
                public byte BitLength;
                public byte CurrentBits;
                public byte TotalBytes;
            }

            private class DictionaryListComparer : Comparer<KeyValuePair<byte, uint>>
            {
                public static DictionaryListComparer Instance = new DictionaryListComparer();

                public override int Compare(KeyValuePair<byte, uint> x, KeyValuePair<byte, uint> y) =>
                    y.Value.CompareTo(x.Value);
            }
        }
    }
}
