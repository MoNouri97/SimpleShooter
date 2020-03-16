using System.Collections;
using System.Collections.Generic;
public static class Utils
{
	public static t[] ShuffleArray<t>(t[] array, int seed)
	{
		System.Random rng = new System.Random(seed);

		for (int i = 0; i < array.Length - 1; i++)
		{
			int randIndex = rng.Next(i, array.Length);
			t temp = array[randIndex];
			array[randIndex] = array[i];
			array[i] = temp;
		}

		return array;
	}
}
