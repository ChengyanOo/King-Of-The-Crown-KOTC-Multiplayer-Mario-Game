using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

public class MapPicker
{
	private int previouschoice;
	private List<string> mapList;
	
	public MapPicker(List<string> mapList)
	{
		this.previouschoice = -1;
        this.mapList = mapList;
	}

	public void setList(List<string> mapList)
	{
		this.mapList = mapList;
	}

	public string Next()
	{
        var random = new Random();
        int index = random.Next(mapList.Count);
        while (index == previouschoice)
		{
            index = random.Next(mapList.Count);
        }
		previouschoice = index;
            
		return mapList[index];
    }
}
