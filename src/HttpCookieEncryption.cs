///Edited from original project at https://github.com/ericnewton76/httpsecurecookie
///Defined new build constants,  NET20 || NET30 || NET35
///30Mar16, Keyur Desai, Infosys

using System;
using System.Web;
using System.Web.Configuration;
using System.Security.Cryptography;

using System.Web.Security;

namespace System.Web
{
	/// <summary>
	/// Provides methods for Encrypting and Decrypting Cookies.
	/// </summary>
	public static class HttpCookieEncryption
	{
		/// <summary>
		/// Decrypts the cookie.
		/// </summary>
		/// <param name="cookie">The cookie to decrypt.</param>
		/// <returns>A new HttpCookie instance with the Value decrypted.</returns>
		public static HttpCookie Decrypt(HttpCookie cookie)
		{
			if(cookie==null) throw new ArgumentNullException("cookie");

			//nothing to do!
			if(cookie.Value.Length==0) return cookie;

			try
            {
#if NET10 || NET11 || NET20 || NET30 || NET35
				byte[] encrypted = MachineKeyWrapper.HexStringToByteArray(cookie.Value);
				if (encrypted == null) return null;	// i wonder if this is the most intuitive situation here... the above method will return null if it cant "DeHex" the string...
				byte[] decrypted = MachineKeyWrapper.EncryptOrDecryptData(false, encrypted, null, 0, encrypted.Length);

				//i wonder if I should guarantee getting a cookie at this point [no exceptions, etc]
				HttpCookie decryptedCookie = CloneCookie(cookie);
			
				decryptedCookie.Value = System.Text.Encoding.Unicode.GetString(decrypted);
				return decryptedCookie;
#else
                HttpCookie decryptedCookie = CloneCookie(cookie);
                decryptedCookie.Value = MachineKeyWrapper.Unprotect(cookie.Value);
                return decryptedCookie;
#endif
			}
			catch(Exception ex)
			{
				//repackage the exception
				throw new HttpException("Unable to Decrypt the cookie.", ex);
			}		
		}
		/// <summary>
		/// Retrieves and decrypts the specified cookie.
		/// </summary>
		/// <param name="context">The current Context.</param>
		/// <param name="cookieName">The name of the Cookie to decrypt.</param>
		/// <returns>An cloned HttpCookie instance with the Value decrypted.</returns>
		public static HttpCookie Decrypt(HttpContext context, string cookieName)
		{
			HttpCookie encrypted = context.Request.Cookies[cookieName];
			if( encrypted == null ) return null;

			return Decrypt(encrypted);
		}
		
		/// <summary>
		/// Returns an encrypted cookie without updating the Response.
		/// </summary>
		/// <param name="source">The cookie to encrypt</param>
		/// <returns>An encrypted instance, cloned from the source.</returns>
		public static HttpCookie Encrypt(HttpCookie source)
		{
			try
            {
#if NET10 || NET11 || NET20 || NET30 || NET35
				byte[] data = System.Text.Encoding.Unicode.GetBytes(source.Value);
				byte[] encData = MachineKeyWrapper.EncryptOrDecryptData(true, data, null, 0, data.Length);

				HttpCookie encrypted = CloneCookie(source);
				encrypted.Value = MachineKeyWrapper.ByteArrayToHexString(encData, encData.Length);
                return encrypted;
#else
                HttpCookie encrypted = CloneCookie(source);
                encrypted.Value = MachineKeyWrapper.Protect(source.Value);
                return encrypted;
#endif

			} 
			catch(Exception ex)
			{
				//repackage
				throw new HttpException("Unable to encrypt the cookie.",ex);
			}
		}
		/// <summary>
		/// Encrypts the specified cookie currently in the Response.
		/// </summary>
		/// <remarks>Note that this method actually changes the cookie currently in the Response.Cookies collection, 
		/// whereas the other Encrypt overload only returns a new instance, cloned from the cookie.</remarks>
		/// <param name="context"></param>
		/// <param name="cookieName"></param>
		/// <returns></returns>
		public static HttpCookie Encrypt(HttpContext context, string cookieName)
		{
			HttpCookie source = context.Response.Cookies[cookieName];
			HttpCookie encryptedCookie = Encrypt(source);
			context.Response.Cookies.Set(encryptedCookie);
			return encryptedCookie;
		}

		private static HttpCookie CloneCookie(HttpCookie source)
		{
			HttpCookie encrypted = new HttpCookie(source.Name);
			encrypted.Expires = source.Expires;
			encrypted.Path = source.Path;
			encrypted.Secure = source.Secure;
			encrypted.Domain = source.Domain;
			encrypted.Value = source.Value; //just being complete.
			return encrypted;
		}

	}
}
