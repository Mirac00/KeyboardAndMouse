namespace KeyboardAndMouse.Models;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; }

    public ICollection<Keyboard> Keyboards { get; set; }
    public ICollection<Mouse> Mice { get; set; }
    public ICollection<Set> Sets { get; set; }
}
