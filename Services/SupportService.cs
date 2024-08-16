using System.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
public class SupportService
{
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

}