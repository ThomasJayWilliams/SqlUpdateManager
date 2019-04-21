namespace SUM.Gates
{
	public interface IBinaryGate : IGate
	{
		bool Request(string output);
	}
}
