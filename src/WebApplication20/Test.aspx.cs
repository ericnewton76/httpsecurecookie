using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication20
{
	public partial class Test : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

			HttpCookie encrypted = Request.Cookies["test"];
			if (encrypted != null)
			{
				HttpCookie decrypted = HttpCookieEncryption.Decrypt(encrypted);

				this.txt1.Text = encrypted.Value;
				this.txt2.Text = decrypted.Value;
			}
		}

		protected void btnSet_Click(object sender,EventArgs e)
		{
			HttpCookie cookie = new HttpCookie("test");
			cookie.Value = this.txtCookieValue.Text;

			cookie = HttpCookieEncryption.Encrypt(cookie);
			Response.Cookies.Set(cookie);
		}

		protected void btnExpire_Click(object sender, EventArgs e)
		{
			HttpCookie cookie = Request.Cookies["test"];
			if (cookie != null)
			{
				cookie.Expires = DateTime.Now;
				Response.Cookies.Set(cookie);
			}
		}
	}
}