namespace KeyboardAndMouse.Models;

public class Set
{
    public int Id { get; set; }
    public int KeyboardId { get; set; }
    public int MouseId { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }

    public virtual Keyboard Keyboard { get; set; }
    public virtual Mouse Mouse { get; set; }
    public virtual Category Category { get; set; }
}




