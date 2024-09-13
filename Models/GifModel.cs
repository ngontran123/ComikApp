namespace TestComikApp.Models;

public class GiftModel
{
    public List<GifDetail> Data {get;set;}
}
public class GifDetail
{
  public ImageDetail Images{get;set;}
}
public class ImageDetail
{
 public FixedHeight Fixed_Height{get;set;}
}

public class FixedHeight
{
    public string Url{get;set;}
}