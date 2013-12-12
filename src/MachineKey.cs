using System;
using System.Reflection;

namespace System.Web.Security
{
	/// <summary>
	/// Provides a hook into the current ASP.Net MachineKey parameters.
	/// </summary>
	public class MachineKeyWrapper
	{
		private static MethodInfo _encOrDecData;

		static MachineKeyWrapper()
		{
#if NET10 || NET11
			//* this whole block hasnt been run tested yet.

			Assembly systemWebAssembly = Assembly.GetAssembly(typeof(System.Web.HttpApplication));
			Type machineKeyType = systemWebAssembly.GetType("System.Web.Configuration.MachineKeySection");

			if (machineKeyType == null)
			{
				// try to get asp.net pre 2.0 type
				machineKeyType = systemWebAssembly.GetType("System.Web.Configuration.MachineKey", false);
			}

			if (machineKeyType == null) throw new InvalidOperationException("Unable to get the core framework type to hook into.");
			*/
#else
			//aspnet 2.0 through 4.5 are using this.
			Type machineKeyType = typeof(System.Web.Configuration.MachineKeySection);
#endif
			
			BindingFlags bf = BindingFlags.NonPublic | BindingFlags.Static;

			_encOrDecData = machineKeyType.GetMethod("EncryptOrDecryptData", bf, null, new Type[] { typeof(bool), typeof(byte[]), typeof(byte[]), typeof(int), typeof(int) }, new ParameterModifier[] { });
			
			//is there any way to get some kind of pointer?  or just trust:
			// MethodBase.Invoke
			// RuntimeMethodInfo.Invoke
			// RuntimeMethodHandle.InvokeFast
			// RuntimeMethodHandle._InvokeFast
			// ...lot of extra calls...

			string exMsg=null;
			if (_encOrDecData == null) exMsg = (exMsg ?? "") + ",EncryptOrDecryptData";
			
			if(exMsg!=null)
			{
				//Log.Error("Unable to get the methods to invoke: " + exMsg.TrimStart(','));
				throw new InvalidOperationException("Unable to get the methods to invoke: " + exMsg.TrimStart(','));
			}
		}

		// for the record, I feel that this MachineKey class should be public.  Why not have us
		// think about security only a little bit, and say, "Here... here's some nice helper
		// methods for you to encrypt data only in a tamper-proof way" 
		// anyways, whatever.

		/// <summary>
		/// Converts a hex string into a byte array.  Original credit to: http://stackoverflow.com/a/311179/323456
		/// </summary>
		/// <param name="str">string to convert</param>
		/// <returns>byte array</returns>
		public static byte[] HexStringToByteArray(string hex)
		{
			int NumberChars = hex.Length / 2;
			byte[] bytes = new byte[NumberChars];
			using (var sr = new System.IO.StringReader(hex))
			{
				for (int i = 0; i < NumberChars; i++)
					bytes[i] =
					  Convert.ToByte(new string(new char[2] { (char)sr.Read(), (char)sr.Read() }), 16);
			}
			return bytes;
		}
		/// <summary>
		/// Converts a byte array into a hex string.  Original credit to: http://social.msdn.microsoft.com/Forums/vstudio/en-US/3928b8cb-3703-4672-8ccd-33718148d1e3/byte-array-to-hex-string?forum=csharpgeneral
		/// </summary>
		/// <param name="array">array to convert</param>
		/// <param name="length">length of array to convert</param>
		/// <returns>hex string representing the byte array.</returns>
		public static string ByteArrayToHexString(byte[] p, int length)
		{
			char[] c = new char[p.Length * 2 + 2];

			byte b;

			c[0] = '0'; c[1] = 'x';

			for (int y = 0, x = 2; y < p.Length; ++y, ++x)
			{

				b = ((byte)(p[y] >> 4));

				c[x] = (char)(b > 9 ? b + 0x37 : b + 0x30);

				b = ((byte)(p[y] & 0xF));

				c[++x] = (char)(b > 9 ? b + 0x37 : b + 0x30);

			}

			return new string(c);
		}
		/// <summary>
		/// Encrypts and decrypts data using ASP.Net MachineKey configuration settings.
		/// </summary>
		/// <param name="encrypting">true if encrypting, false if decrypting</param>
		/// <param name="data">the data to operate on</param>
		/// <param name="mod"></param>
		/// <param name="index">beginning index</param>
		/// <param name="length">length of array to operate on</param>
		/// <returns>encrypted or decrypted byte array</returns>
		public static byte[] EncryptOrDecryptData(bool encrypting, byte[] data, byte[] mod, int index, int length)
		{
			return (byte[])_encOrDecData.Invoke(null, new object[] { encrypting, data, mod, index, length });
		}

	}
}
