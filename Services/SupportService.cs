using System.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using System.Management;
using Microsoft.Playwright;
using Microsoft.EntityFrameworkCore.ValueGeneration;
using MimeKit.Cryptography;
using Microsoft.VisualBasic;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using Castle.Components.DictionaryAdapter;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TestComikApp.Service;

public class SupportService
{

 List<string[]> list_comik_table=new List<string[]>();
 List<string[]> list_author_table = new List<string[]>();

 List<string[]> list_genre_table = new List<string[]>();

 List<string[]> list_comik_author_table = new List<string[]>();
 List<string[]> list_comik_genre_table = new List<string[]>();

 List<string[]> list_chapter_table = new List<string[]>();

 List<string[]> list_chapter_detail = new List<string[]>();

 int latest_comik_item=0;
 int latest_author_item=0;
 int latest_genre_item=0;

int latest_comik_author_item=0;

int latest_comik_genre_item = 0;

int latest_chapter_item=0;

int latest_chapter_detail_item = 0;


     public string AddSha256(string data)
 {
    using(SHA256 hash=SHA256.Create())
    {
        byte[] bytes=hash.ComputeHash(Encoding.UTF8.GetBytes(data));

        StringBuilder sha_hash=new StringBuilder();
        
        for(int i=0;i<bytes.Length;i++)
        {
            sha_hash.Append(bytes[i].ToString("x2"));
        }
        return sha_hash.ToString();
    }
 }

 public string GetCurrentFilePath(string file_name)
 {
    string full_file_path=Path.Combine(Directory.GetCurrentDirectory(),file_name);
    return full_file_path;
 }

 public void fillFullListData(string file_name,List<string[]>list_data)
 {
    try
    {
    string file_path=GetCurrentFilePath(file_name);
    if(!File.Exists(file_path))
    {
        return;
    }   
    using(StreamReader srd=new StreamReader(file_path,Encoding.UTF8))
    {
        while(!srd.EndOfStream)
        {
            string? line=srd.ReadLine();
            list_data.Add(line.Split(','));
        }
    }
    }
    catch(Exception er)
    {
        Console.WriteLine(er.Message);
    }
 }

 public string generateRandomPassword()
{
string guid=Guid.NewGuid().ToString();
return guid;
}

public string getCurrentOs()
{
var os_name = (from x in new ManagementObjectSearcher("SELECT Caption FROM Win32_OperatingSystem").Get().Cast<ManagementObject>()
                      select x.GetPropertyValue("Caption")).FirstOrDefault();


return os_name != null ? os_name.ToString() : "Unknown";
}

public string standardString(string value)
{
    return value.Replace("\n","").Replace("\r","").Replace("\t","").TrimStart().TrimEnd();
}


public void writeCsvFile(string file_name,string data)
{
    try
    {
    string file_path=GetCurrentFilePath(file_name);
    using(var writer=new StreamWriter(file_path,true,Encoding.UTF8))
    {
        writer.WriteLine(data);
    }
    }
    catch(Exception ex)
    {

    }
}

public string handleEscapedField(string data)
{
    if(data.Contains(",")||data.Contains(";")||data.Contains("\""))
    {
        data=data.Replace("\"","\"\"");
        return $"\"{data}\"";
    }
    return data;
}

public int getLatestId(List<string[]>list_data)
{  int latest_row=0;
    try
    {

     if(list_data.Count>0)
     {
       string[] data=list_data[list_data.Count-1];
       latest_row=Convert.ToInt32(data[0]);  
     }
     else
     {
        latest_row=0;
     }
    }
    catch(Exception er)
    {
    Console.WriteLine(er.Message);
    }
    return latest_row;
}


public async Task<int> addComikTable(IPage page,ILocator item,IReadOnlyList<ILocator> line_content,int id)
{     int res=0;
      var chapter_content=await item.Locator(".chapter-name").TextContentAsync();
            string chapters= standardString(chapter_content);
            var line_content_first=line_content[0];
            var line_content_second=line_content[2];
            var line_content_third = line_content[3];
            var line_content_four= line_content[4];
            var description_content=await page.Locator(".dbs-content").TextContentAsync();
            string description=$"\"{standardString(description_content)}\"";            
            var comik_name=await line_content_first.Locator(".manga-name").TextContentAsync();
            var status = await line_content_second.Locator(".result").TextContentAsync();
            var published=await line_content_third.Locator(".result").TextContentAsync();
            var views=await line_content_four.Locator(".result").TextContentAsync();
            string views_parse=standardString(views);
            string status_comik=standardString(status);
            string published_date=standardString(published);
            string comik_name_parse =standardString(comik_name);            
            var avatar = await page.QuerySelectorAsync($"img[alt=\"{comik_name_parse}\"]");
            var link_avatar=await avatar.GetAttributeAsync("src");
            string file_name="comik.csv";
            if(this.list_comik_table.Any(comik=>comik[1]==comik_name_parse))
            {   res=-1;
                return res;
            }
            string data=$"{id},{this.handleEscapedField(comik_name_parse)},{this.handleEscapedField(published_date)},{this.handleEscapedField(chapters)},{this.handleEscapedField(status_comik)},{this.handleEscapedField(description)},{this.handleEscapedField(views_parse)},white,{this.handleEscapedField(link_avatar)}";
            this.list_comik_table.Add(data.Split(','));
            writeCsvFile(file_name,data);
            Console.WriteLine($"{comik_name_parse} & {link_avatar}");
            Console.WriteLine($"Chapter:{chapters}");
            Console.WriteLine($"Description:{description}");
            Console.WriteLine($"Status:{status_comik}");
            Console.WriteLine($"Published:{published_date}");
            return res;
}

public string standardAuthorName(string author)
{   string standard_author_name="";
    author=author.Trim();
    if(author.Contains(","))
    {
        string[] value_split=author.Split(',');
        for(int i=value_split.Length-1;i>=0;i--)
        {
            standard_author_name+=value_split[i]+" ";
        }
    }
    else 
    {
        standard_author_name=author;
    }
    return standard_author_name.TrimStart().TrimEnd();
}

public async Task addAuthorTable(IReadOnlyList<ILocator> line_contents)
{ 

 var line_content=line_contents[1];
 var authors_detail_section=line_content.Locator(".result").First;
 var authors_detail=await authors_detail_section.Locator("a").AllAsync();
 string author_name_parse="";
 string file_name="author.csv";
 if(authors_detail!=null)
 { 
 foreach(var author in authors_detail)
 {   
    var author_name=await author.TextContentAsync();
    author_name_parse = standardString(author_name);
    author_name_parse=standardAuthorName(author_name_parse);
    if(this.list_author_table.Any(author=>author[1]==author_name_parse))
    {  string[] author_exist= this.list_author_table.SingleOrDefault(author=>author[1]==author_name_parse);
        int author_id=Convert.ToInt32(author_exist[0]);
        this.addComikAuthorDetail(this.latest_comik_item,author_id);
        return;
    }
    string data=$"{this.latest_author_item},{this.handleEscapedField(author_name_parse)},{""}";
    this.list_author_table.Add(data.Split(','));
    this.writeCsvFile(file_name,data);
    this.addComikAuthorDetail(latest_comik_item,latest_author_item);
    this.latest_author_item+=1;
    Console.WriteLine(author_name_parse);
 }
 }
} 

public async Task addChapterDetail(IPage page,int chapter_id,string filename)
{
    
    var scroll_script=@"
    const autoScroll = async () => {
         const scrollStep = 3000; 
         const scrollDelay = 100; 

            return new Promise((resolve, reject) => {
                const scrollInterval = setInterval(() => {
                    window.scrollBy(0, scrollStep);
                    if ((window.innerHeight + window.scrollY) >= document.body.offsetHeight) {
                        clearInterval(scrollInterval);
                        resolve();
                    }
                }, scrollDelay);
            });      
        }
        autoScroll();
    ";
    await Task.Delay(5000);
   await page.EvaluateAsync(scroll_script);
   int count_nums=0;
   var images=await page.Locator(".image-vertical").AllAsync();
   
  if(images!=null)
  {
   foreach(var image in images)
   {  count_nums+=1;
    var image_href=await image.GetAttributeAsync("src");
    this.latest_chapter_detail_item+=1;
    string data_detail=$"{latest_chapter_detail_item},{chapter_id},{count_nums},{this.handleEscapedField(image_href)}";
    this.writeCsvFile(filename,data_detail);
    Console.WriteLine(image_href);
   }
  }
// if(images!=null)
// {
//     Console.WriteLine("yesh");
// }
// else{
//     Console.WriteLine("no");
// }
}

public async Task addGenreTable(IReadOnlyList<ILocator> line_contents)
{
try{
  var line_content=line_contents[5];
  var genre_detail_section=line_content.Locator(".result").First;
  var genre_detail=await line_content.Locator("a").AllAsync();
  string genre_parse="";
  string file_name="genres.csv";
  foreach(var genre in genre_detail)
  {
    var genre_name=await genre.TextContentAsync();
    
    genre_parse=standardString(genre_name);

    if(this.list_genre_table.Any(genre=>genre[1]==genre_parse))
    {  
      string[] current_genres=this.list_genre_table.SingleOrDefault(genre_item=>genre_item[1]==genre_parse);
      int genre_id=Convert.ToInt32(current_genres[0]);
      this.addComikGenreDetail(latest_comik_item,genre_id);
        return;
    } 

    string data=$"{this.latest_genre_item},{this.handleEscapedField(genre_parse)}";
    list_genre_table.Add(data.Split(','));
    this.writeCsvFile(file_name,data);
    this.addComikGenreDetail(latest_comik_item,latest_genre_item);
    this.latest_genre_item+=1;
    Console.WriteLine(genre_parse);
  }
}
catch(Exception er)
{
}
}

public async Task addComikAuthorDetail(int comik_id,int author_id)
{ string file_name="comik_author.csv";
if(this.list_comik_author_table.Any(item=>item[1]==comik_id.ToString()&&item[2]==author_id.ToString()))
{
    return;
}
  latest_comik_author_item+=1;
  string data=$"{latest_comik_author_item},{comik_id},{author_id}";
  this.writeCsvFile(file_name,data);
}

public async Task addComikGenreDetail(int comik_id,int genre_id)
{
    string file_name="comik_genre.csv";
    if(this.list_comik_genre_table.Any(item=>item[1]==comik_id.ToString() && item[2]==genre_id.ToString()))
    {
        return;
    }
    this.latest_comik_genre_item+=1;
    string data=$"{this.latest_comik_genre_item},{comik_id},{genre_id}";
    this.writeCsvFile(file_name,data);
}


public async Task webScrapingTesting()
{    
    string comik_table="comik.csv";

    string author_table = "author.csv";

    string genre_table = "genres.csv";

    string comik_author_table = "comik_author.csv";

    string comik_genre_table = "comik_genre.csv";

    string chapter_table = "chapter.csv";

    string chapter_detail_table = "chapter_detail.csv";
       
    fillFullListData(comik_table,list_comik_table);

    fillFullListData(author_table,list_author_table);

    fillFullListData(genre_table,list_genre_table);

    fillFullListData(comik_author_table,list_comik_author_table);  

    fillFullListData(comik_genre_table,list_comik_genre_table);

    fillFullListData(chapter_table,list_chapter_table);

    fillFullListData(chapter_detail_table,list_chapter_detail);  
    
    using(var playwright=await Playwright.CreateAsync())
    {
           
        var browser=await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions{Headless=false});

        var page = await browser.NewPageAsync();

             await page.AddInitScriptAsync(@"
            document.addEventListener('contextmenu', event => event.stopPropagation(), true);
            document.body.oncontextmenu = null;
        ");  
                     
        await page.GotoAsync("https://mangakakalot.to/new");
       // await page.GotoAsync("https://mangakakalot.to/read/the-first-times-lady-68337/en/chapter-1");

        await page.WaitForLoadStateAsync(LoadState.NetworkIdle);

        //await addChapterDetail(page);                        
        var items = await page.Locator(".item").AllAsync();
        
        latest_comik_item=getLatestId(list_comik_table)+1;

        latest_author_item=getLatestId(list_author_table)+1;

        latest_genre_item=getLatestId(list_genre_table)+1;

        latest_comik_author_item=getLatestId(list_comik_author_table);

        latest_comik_genre_item=getLatestId(list_comik_genre_table);

        latest_chapter_item=getLatestId(list_chapter_table);

        Console.WriteLine("latest id:"+latest_comik_item);

        foreach(var item in items)
        {   //await addComikTable(page,item);
         var link =  item.Locator(".manga-poster").First;         

         await link.ClickAsync();
         
         var line_content=await page.Locator(".line-content").AllAsync();

        int val= await addComikTable(page,item,line_content,latest_comik_item);

         await addAuthorTable(page,line_content);

         await addGenreTable(line_content);

        await Task.Delay(2000);
         
        var dropdown = page.Locator("#chap-lang");
        await dropdown.ClickAsync();

        var element = page.Locator("a[data-code='en'][data-type='chap']");

        await element.ClickAsync();

         var chapters=page.Locator(".clb-ul").First;

         var chaptr=await chapters.Locator(".item").AllAsync();
        
        foreach(var chapter in chaptr)
        {
            var chapter_no=await chapter.Locator(".chapter-name").TextContentAsync();
            var chapter_link=chapter.Locator("a").First;
            string chapter_no_parse=standardString(chapter_no);
            DateTime date_time_now=DateTime.Now;
            string date_added = date_time_now.ToString("dd/MM/yyyy HH:mm:ss");
            if(this.list_chapter_table.Any(chapter=>chapter[1]==chapter_no_parse && chapter[2]==latest_comik_item.ToString()))
            {
                break;
            }
            latest_chapter_item+=1;
            string chapter_data=$"{latest_chapter_item},{chapter_no_parse},{latest_comik_item},{this.handleEscapedField(date_added)}";
            
            this.writeCsvFile(chapter_table,chapter_data);
          
            await chapter_link.ClickAsync();
            await addChapterDetail(page,latest_chapter_item,chapter_detail_table);
            await Task.Delay(100);
        }
         if(val==0)
         {
          latest_comik_item+=1;
         }     
    
       // await addGenreTable(page,line_content);
         await page.GoBackAsync();
        }
        // await page.GetByRole(AriaRole.Link,new(){Name="Browse"}).ClickAsync();
        // await page.ScreenshotAsync(new PageScreenshotOptions{Path="screenshot.png"});    
        await browser.CloseAsync();        
    }
}

}