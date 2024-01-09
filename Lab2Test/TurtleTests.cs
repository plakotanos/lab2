using Lab2;

namespace Lab2Test;

public class TurtleTests
{
    private Turtle _turtle = null!;
    
    [SetUp]
    public void Setup()
    {
        _turtle = new();
    }

    [Test]
    public void AngleCommand_RotatesTurtle()
    {
        _turtle.Turn(180);
        
        Assert.That(_turtle.Direction, Is.EqualTo(180));
    }
    
    
    [Test]
    public void AngleCommand_TurnsPast360()
    {
        _turtle.Turn(361);
        
        Assert.That(_turtle.Direction, Is.EqualTo(1));
    }
    
    [Test]
    public void MoveCommand_ChangesX()
    {
        _turtle.Move(1);
        
        Assert.That(_turtle.X, Is.EqualTo(1));
    }
    
    [Test]
    public void MoveCommand_ChangesY()
    {
        _turtle.Turn(90);
        _turtle.Move(1);
        
        Assert.That(_turtle.Y, Is.EqualTo(1));
    }

    public void MoveCommand_TracksSteps()
    {
        int stepCount = _turtle.Steps.Count;
        
        _turtle.Move(1);
        
        Assert.That(_turtle.Steps, Has.Count.EqualTo(stepCount + 1));
    }
    
    [Test]
    public void MoveCommand_DoesNotCreateLines_WhenPenIsUp()
    {
        int pathLength = _turtle.Path.Count;
        
        _turtle.PenUp();
        _turtle.Move(1);

        Assert.That(_turtle.Path, Has.Count.EqualTo(pathLength));
    }
    
    [Test]
    public void MoveCommand_CreatesLines_WhenPenIsDown()
    {
        int pathLength = _turtle.Path.Count;
        
        _turtle.PenDown();
        _turtle.Move(1);

        Assert.That(_turtle.Path, Has.Count.EqualTo(pathLength + 1));
    }
    
    [Test]
    public void DrawingFigure_CreatesFigure()
    {
        int figureCount = _turtle.Figures.Count;
        
        _turtle.PenDown();
        _turtle.Move(1);
        _turtle.Turn(90);
        _turtle.Move(1);
        _turtle.Turn(90);
        _turtle.Move(1);
        _turtle.Turn(90);
        _turtle.Move(1);

        Assert.That(_turtle.Figures, Has.Count.EqualTo(figureCount + 1));
    }
    
    [Test]
    public void DrawingFigure_ResetsPath()
    {
        _turtle.PenDown();
        _turtle.Move(1);
        _turtle.Turn(90);
        _turtle.Move(1);
        _turtle.Turn(90);
        _turtle.Move(1);
        _turtle.Turn(90);
        _turtle.Move(1);

        Assert.That(_turtle.Path, Is.Empty);
    }
}