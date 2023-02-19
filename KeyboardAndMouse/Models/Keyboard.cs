namespace KeyboardAndMouse.Models;

public class Keyboard
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int CategoryId { get; set; }
    public string Description { get; set; }
    public int Rating { get; set; }

    public Category Category { get; set; }
    public ICollection<Set> Sets { get; set; }
}



