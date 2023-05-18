using System;
using System.IO;
using System.Text;
using System.Xml;

public class XmlFileSkeletonGenerator
{
    private XmlWriter writer;
	private string filePath, levelName;
    private int levelColumns, levelRows, screenWidth, screenHeight, tileWidth, tileHeight;
    XmlWriterSettings settings = new XmlWriterSettings() { Indent = true, };

    public XmlFileSkeletonGenerator(string filePath, string levelName, int levelColumns, int levelRows, 
        int screenWidth, int screenHeight, int tileWidth, int tileHeight)
    {
        this.filePath = filePath;
        FileStream fileStream = new FileStream(filePath, FileMode.Create, FileAccess.Write);
        writer = XmlWriter.Create(fileStream, settings);

        this.levelName = levelName;
        this.levelColumns = levelColumns;
        this.levelRows = levelRows;
        this.screenWidth = screenWidth;
        this.screenHeight = screenHeight;
        this.tileWidth = tileWidth;
        this.tileHeight = tileHeight;
    }

    public void CreateLevelDocument()
    {
        writer.WriteStartDocument();
        writer.WriteStartElement("Level");

        //write level attributes
        WriteElement("Title", levelName);
        WriteElement("LevelColumns", levelColumns);
        WriteElement("LevelRows", levelRows);
        WriteElement("ScreenWidth", screenWidth);
        WriteElement("ScreenHeight", screenHeight);
        WriteElement("TileWidth", tileWidth);
        WriteElement("TileHeight", tileHeight);

        /*
        //write layer 0.0
        writer.WriteStartElement("Layer");
        writer.WriteAttributeString("parallax", "0.5");
        for (int i = 0; i < levelColumns; i++)
        {
            writer.WriteStartElement("Column");
            writer.WriteAttributeString("x", i.ToString());
            writer.WriteEndElement();
        }
        writer.WriteEndElement();
        */

        //write layer 0.1
        writer.WriteStartElement("Layer");
        writer.WriteAttributeString("parallax", "1.0");
        for (int i = 0; i < levelColumns; i++)
        {
            writer.WriteStartElement("Column");
            writer.WriteAttributeString("x", i.ToString());
            if(i==1)
            {
                WritePlayer(levelRows - 3, "small", "idle", "player1");
            } else if(i==levelColumns - 2)
            {
                WritePlayer(levelRows - 3, "small", "idle", "player2");
            }
            WriteGround(levelRows);
            writer.WriteEndElement();
        }
        writer.WriteEndElement();

        //finish document
        writer.WriteEndDocument();
        writer.Flush();
        writer.Close();
    }

    public void WriteElement(string name, int value)
    {
        writer.WriteStartElement(name);
        writer.WriteValue(value);
        writer.WriteEndElement();
    }

    public void WriteElement(string name, string value)
    {
        writer.WriteStartElement(name);
        writer.WriteValue(value);
        writer.WriteEndElement();
    }

    public void WriteGround(int levelRows)
    {
        WriteSprite("block", levelRows - 2, "floor", "top", null);
        WriteSprite("block", levelRows - 1, "floor", "bottom", null);
    }

    public void WriteSprite(string entityType, int height, string type)
    {
        writer.WriteStartElement(entityType);
        writer.WriteAttributeString("y", height.ToString());
        writer.WriteAttributeString("type", type);
        writer.WriteEndElement();
    }

    public void WriteSprite(string entityType, int height, string type, string verticalOrient, string horizontalOrient)
    {
        writer.WriteStartElement(entityType);
        writer.WriteAttributeString("y", height.ToString());
        writer.WriteAttributeString("type", type);
        if(verticalOrient != null)
        {
            writer.WriteAttributeString("vertical", verticalOrient);
        }
        if(horizontalOrient != null)
        {
            writer.WriteAttributeString("horizontal", horizontalOrient);
        }
        writer.WriteEndElement();
    }

    public void WritePlayer(int height, string power, string action, string variant)
    {
        writer.WriteStartElement("player");
        writer.WriteAttributeString("y", height.ToString());
        writer.WriteAttributeString("power", power);
        writer.WriteAttributeString("action", action);
        writer.WriteAttributeString("variant", variant);
        writer.WriteEndElement();
    }
}
