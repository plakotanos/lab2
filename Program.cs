using System;
using System.Collections.Generic;

class TurtleGraphics
{
    static void Main()
    {
        Console.WriteLine("Turtle Graphics Simulator");
        Console.WriteLine("Commands:");
        Console.WriteLine("move N: command to change turtle’s position on N steps.");
        Console.WriteLine("angle N: command to change turtle’s angle of direction to N degrees.");
        Console.WriteLine("pd: command to put down the pen.");
        Console.WriteLine("pu: command to put up the pen.");
        Console.WriteLine("color {colorName}: command to change turtle’s color of the pen to {colorName} color. Possible values: black, green.");
        Console.WriteLine("list steps: command to show all executed steps.");
        Console.WriteLine("list figures: command to show all properties of completed figures.");
        Console.WriteLine("exit: command to exit the program.");
        Console.WriteLine("Current color: black, pen state: put up, location (0; 0), direction: 0 degrees.");

        Turtle turtle = new Turtle();

        while (true)
        {
            Console.Write("> ");
            string input = Console.ReadLine();
            string[] parts = input.Split(' ');

            // Проверка и выполнение команд pu, pd и exit
            switch (parts[0])
            {
                case "pu":
                    turtle.PenUp();
                    Console.WriteLine(turtle);
                    continue;
                case "pd":
                    turtle.PenDown();
                    Console.WriteLine(turtle);
                    continue;
                case "exit":
                    return;
            }

            if (parts.Length < 2)
            {
                Console.WriteLine("Invalid command. Please use the correct format.");
                continue;
            }
            string command = parts[0];
            switch (command)
            {
                case "move":
                    if (int.TryParse(parts[1], out int steps))
                    {
                        turtle.Move(steps);
                        Console.WriteLine(turtle);
                    }
                    else
                    {
                        Console.WriteLine("Invalid argument for move command. Please use a valid integer.");
                    }
                    break;
                case "angle":
                    if (int.TryParse(parts[1], out int angle))
                    {
                        turtle.Turn(angle);
                        Console.WriteLine(turtle);
                    }
                    else
                    {
                        Console.WriteLine("Invalid argument for angle command. Please use a valid integer.");
                    }
                    break;
                case "color":
                    if (parts.Length < 2)
                    {
                        Console.WriteLine("Invalid argument for color command. Please specify a color (black or green).");
                    }
                    else
                    {
                        string colorName = parts[1];
                        if (colorName == "black" || colorName == "green")
                        {
                            turtle.SetColor(colorName);
                            Console.WriteLine(turtle);
                        }
                        else
                        {
                            Console.WriteLine("Invalid color name. Please use 'black' or 'green'.");
                        }
                    }
                    break;
                case "list":
                    if (parts.Length < 2)
                    {
                        Console.WriteLine("Invalid argument for list command. Please specify 'steps' or 'figures'.");
                    }
                    else
                    {
                        string listType = parts[1];
                        if (listType == "steps")
                        {
                            turtle.ListSteps();
                        }
                        else if (listType == "figures")
                        {
                            ListFigures(turtle.figures);
                        }
                        else
                        {
                            Console.WriteLine("Invalid list type. Please use 'steps' or 'figures'.");
                        }
                    }
                    break;
                default:
                    Console.WriteLine("Invalid command. Please use one of the supported commands.");
                    break;
            }
        }
    }

    static void ListFigures(List<Figure> figures)
    {
        Console.WriteLine("List of completed figures:");
        foreach (Figure figure in figures)
        {
            Console.WriteLine(figure);
        }
    }
}

class Turtle
{
    private int x;
    private int y;
    private int direction;
    private bool penDown;
    private string color;
    private List<Step> steps;
    private List<Step> path;
    public List<Figure> figures { get; set; }


    public Turtle()
    {
        x = 0;
        y = 0;
        direction = 0;
        penDown = false;
        color = "black";
        steps = new List<Step>();
        path = new List<Step>();
        figures = new List<Figure>();
    }

    public void Move(int stepsCount)
    {
        int newX = x + (int)(stepsCount * Math.Cos(direction * Math.PI / 180));
        int newY = y + (int)(stepsCount * Math.Sin(direction * Math.PI / 180));
        Step step = new Step(x, y, newX, newY, penDown, color);

        steps.Add(step);
        if (penDown)
        {
            path.Add(step);
        }
        x = newX;
        y = newY;

        if (Intersects(step))
        {
            Figure newFigure = new Figure(path);
            figures.Add(newFigure);
            PenUp();
            PenDown();
        }
    }

    public void Turn(int angle)
    {
        direction = (direction + angle) % 360;
    }

    public void PenDown()
    {
        penDown = true;
    }

    public void PenUp()
    {
        path = new List<Step>();
        penDown = false;
    }

    public bool PenIsDown()
    {
        return penDown;
    }

    public void SetColor(string newColor)
    {
        color = newColor;
    }

    public string GetColor()
    {
        return color;
    }

    public List<(int, int)> GetSnapshot()
    {
        List<(int, int)> snapshot = new List<(int, int)>();
        foreach (Step step in steps)
        {
            if (step.PenDown)
            {
                snapshot.Add((step.StartX, step.StartY));
            }
        }
        return snapshot;
    }

    private bool Intersects(Step step)
    {
        for (int i = 0; i < path.Count - 1; i++)
        {
            int x1 = path[i].StartX;
            int y1 = path[i].StartY;
            int x2 = path[i].EndX;
            int y2 = path[i].EndY;

            if (DoLinesIntersect(x1, y1, x2, y2, step.StartX, step.StartY, step.EndX, step.EndY))
            {
                return true;
            }
        }
        return false;
    }

    private bool DoLinesIntersect(int x1, int y1, int x2, int y2, int x3, int y3, int x4, int y4)
    {
        // Вычисляем векторы направления для обеих линий.
        int dx1 = x2 - x1;
        int dy1 = y2 - y1;
        int dx2 = x4 - x3;
        int dy2 = y4 - y3;

        // Вычисляем вектор между началом первой линии и началом второй линии.
        int dx3 = x1 - x3;
        int dy3 = y1 - y3;

        // Вычисляем определитель матрицы для системы линейных уравнений.
        int determinant = dx1 * dy2 - dx2 * dy1;

        // Проверяем, являются ли линии параллельными (определитель равен 0).
        if (determinant == 0)
        {
            return false;
        }

        // Вычисляем параметры t1 и t2 для точек пересечения.
        double t1 = (dx2 * dy3 - dx3 * dy2) / (double)determinant;
        double t2 = (dx1 * dy3 - dx3 * dy1) / (double)determinant;

        // Проверяем, лежат ли точки пересечения внутри отрезков.
        if (t1 >= 0 && t1 <= 1 && t2 >= 0 && t2 <= 1)
        {
            if (x2 == x3 && y2 == y3) return false;
            return true; // Прямые пересекаются.
        }

        return false; // Прямые не пересекаются.
    }

    public void ClearSteps()
    {
        steps.Clear();
    }

    public void ListSteps()
    {
        Console.WriteLine("List of executed steps:");
        foreach (Step step in steps)
        {
            Console.WriteLine(step);
        }
    }

    public override string ToString()
    {
        string penState = penDown ? "put down" : "put up";
        return $"Current color: {color}, pen state: {penState}, location ({x}; {y}), direction: {direction} degrees.";
    }
}

class Step
{
    public int StartX { get; private set; }
    public int StartY { get; private set; }
    public int EndX { get; private set; }
    public int EndY { get; private set; }
    public bool PenDown { get; private set; }
    public string Color { get; private set; }

    public Step(int startX, int startY, int endX, int endY, bool penDown, string color)
    {
        StartX = startX;
        StartY = startY;
        EndX = endX;
        EndY = endY;
        PenDown = penDown;
        Color = color;
    }

    public override string ToString()
    {
        string penState = PenDown ? "down" : "up";
        return $"({StartX}; {StartY}) -> ({EndX}; {EndY}), pen {penState}, color: {Color}";
    }
}

class Figure
{
    public List<Step> steps { get; private set; }

    public Figure(List<Step> steps)
    {
        this.steps = steps;
    }


    public override string ToString()
    {
        string verticesStr = "Figure with lines:\n";
        foreach (Step step in steps)
        {
            verticesStr += ("(");
            verticesStr += step.StartX;
            verticesStr += "; ";
            verticesStr += step.StartY;
            verticesStr += ")--->(";
            verticesStr += step.EndX;
            verticesStr += "; ";
            verticesStr += step.EndY;
            verticesStr += ")  ";
            verticesStr += step.Color;
            verticesStr += "\n";
        }

        return verticesStr;
    }
}

