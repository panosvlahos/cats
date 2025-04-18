using System;
namespace Contracts;
public class CatDto
{
    public string Id { get; set; }
    public string Url { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public List<BreedDto> Breeds { get; set; }
}
public class BreedDto
{
    public string Temperament { get; set; }
}
