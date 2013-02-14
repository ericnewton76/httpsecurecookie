httpsecurecookie
================

Store encrypted data within encoded cookies for use in server-side implementations without requiring sessions.

I originally wrote about the need for encrypted cookies a while back, 2006 to be accurate:
http://www.codeproject.com/Articles/8742/Encrypting-Cookies-to-prevent-tampering

Some sample code to illustrate how easy it is to encrypt:

```c#
void Set_CookieValue()
{
    HttpCookie cookie = new HttpCookie("protect");
    cookie.Value = "protect_this_value";

    Response.Cookies.Set(HttpCookieEncryption.Encrypt(cookie));
}

void Get_CookieValue()
{
    HttpCookie protected = HttpCookieEncryption.Decrypt(Request.Cookies["protect"]);

	string protected_And_tamperProof_Value = protected.Value;
}