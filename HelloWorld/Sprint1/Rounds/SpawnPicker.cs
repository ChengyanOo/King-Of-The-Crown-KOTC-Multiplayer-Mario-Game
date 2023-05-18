using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class SpawnPicker
{
	private int previouschoice;
	private List<Point> spawnList;
	
	public SpawnPicker(List<Point> spawnList)
	{
		this.previouschoice = -1;
        this.spawnList = spawnList;
	}

	public void setList(List<Point> spawnList)
	{
		this.spawnList = spawnList;
	}

	public Vector2 Next()
	{
        var random = new Random();
        int index = random.Next(spawnList.Count);
        while (index == previouschoice)
		{
            index = random.Next(spawnList.Count);
        }
		previouschoice = index;
            
		return spawnList[index].ToVector2();
    }
}
