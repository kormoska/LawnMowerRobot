using System;
using System.Collections.Generic;

namespace LawnMowerRobot {

  public class Garden
  {
    private string[] GardenMap {get;set;}
    private Coords Charger;
    private Dictionary<Coords, bool> Borders;

    public Garden(string[] map, Coords charger)
    {
        Charger = charger;
        GardenMap = map;
        char[] robotline = GardenMap[Charger.Y].ToCharArray();
        robotline[Charger.X] = 'R';
        GardenMap[Charger.Y] = new String(robotline);
        Borders = new Dictionary<Coords, bool> ();

        for (int i=0; i<GardenMap.Length; i++)
        {
            for (int j=0; j<GardenMap[i].Length; j++)
            {
                if (GardenMap[i][j]=='X')
                {
                    Borders.Add(new Coords(j,i),false);
                }
            }
        }
    }

    public bool isBorder(Coords position)
    {
        if (Borders.ContainsKey(position))
        {
            Borders[position] = true;
            return true;
        } else
        {
            return false;
        }
    }

    public void RobotMoved(Coords coordfrom, Coords coordto, int fromvalue)
    {
        char[] robotline = GardenMap[coordfrom.Y].ToCharArray();

        robotline[coordfrom.X] = fromvalue.ToString()[0];
        GardenMap[coordfrom.Y] = new String(robotline);
        robotline = GardenMap[coordto.Y].ToCharArray();
        robotline[coordto.X] = 'R';
        GardenMap[coordto.Y] = new String(robotline);
    }
    public void Draw()
    {
        Console.Clear();
        for (int i = 0; i< GardenMap.Length; i++)
        {
            for (int j = 0; j < GardenMap[i].Length; j++)
            {
                if (GardenMap[i][j] =='R')
                {
                    Console.ForegroundColor = ConsoleColor.Blue;
                    Console.Write(GardenMap[i][j]);
                    Console.ResetColor();
                } else if (GardenMap[i][j] =='X')
                {
                    Coords temp = new Coords(j,i);
                   if (Borders.ContainsKey(temp) && Borders[temp])
                   {
                      Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(GardenMap[i][j]);
                        Console.ResetColor();
                    } else {
                        Console.Write(GardenMap[i][j]);
                    }
                } else {
                    Console.Write(GardenMap[i][j]);
                }
            }
            Console.WriteLine();
        }
    }
  }

  public class Robot
  {
    public Coords Position{get; set;}
    public Coords PreviousPosition{get; set;}
    public int counter;
    private Coords Charger{ get; init;}
    private Dictionary<Coords,int> RobotMap;

    public Robot(Coords charger)
    {
        Charger = charger;
        Position = Charger;
        PreviousPosition = Charger;
        counter = 0;
        RobotMap = new Dictionary<Coords,int>();
        RobotMap.Add(Position,1);
    }

    public void Move(Garden garden)
    {
        Coords left = Position + new Coords(-1,0);
        Coords right = Position + new Coords(1,0);
        Coords top = Position + new Coords(0,-1);
        Coords bottom = Position + new Coords(0,1);
        Coords moveto = left;

        int minvalue = Examine(left,garden);

        int tempvalue = Examine(top, garden);
        if (tempvalue < minvalue)
        {
            minvalue = tempvalue;
            moveto = top;
        }

        tempvalue = Examine(right, garden);
        if (tempvalue < minvalue)
        {
            minvalue = tempvalue;
            moveto = right;
        }
         
        tempvalue = Examine(bottom, garden);
        if (tempvalue < minvalue)
        {
            minvalue = tempvalue;
            moveto = bottom;
        }

        RobotMap[moveto]++;
        counter = RobotMap[moveto];
        PreviousPosition = Position;
        Position = moveto;
    }
    private int Examine(Coords cord, Garden garden)
    {
        if (!RobotMap.ContainsKey(cord))
        {
            if (garden.isBorder(cord)){
                RobotMap.Add(cord,int.MaxValue);
            } else {
                RobotMap.Add(cord,0);
            }
        }
        return RobotMap[cord];
    }
  }
  class Program {
    public static void Main(string[] args)
    {
        var initGardenMap = new string[]
        {
            
            "XXXXXXXXXXXX",
            "X          X",
            "X          X",
            "X   XX   XXX",
            "X   XX   X",
            "X        X",
            "X     X  X",
            "X     XX X",
            "X   X    X",
            "XXXXXXXXXX"

        };
        Coords charger = new Coords(4,2);
        Garden garden = new Garden(initGardenMap, charger);
        Robot robot = new Robot(charger);

        garden.Draw();
        do
        {
            Thread.Sleep(200);
            robot.Move(garden);
            garden.RobotMoved(robot.PreviousPosition, robot.Position,robot.counter);
            garden.Draw();

        } while (robot.counter < 4 || robot.Position != charger);
        
    }   
  }
}
