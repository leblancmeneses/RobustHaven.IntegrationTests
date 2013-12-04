namespace ResultDiff.Models
{
	public class Diff<T> where T : class 
	{
		public T Left { get; set; }
		public T Right { get; set; }

		public bool IsEqual()
		{
			if (Left == null)
				return false;

			return Left.Equals(Right);
		}
	}
}
