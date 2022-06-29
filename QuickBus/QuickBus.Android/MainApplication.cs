#if DEBUG
using Android.App;
using Android.Runtime;
using Plugin.CurrentActivity;
using System;

[Application(Debuggable = true)]
#else
	[Application(Debuggable = false)]
#endif
[MetaData("com.google.android.maps.v2.API_KEY", Value = "AIzaSyB56WDRUGS46as0Vrw_9UxtPiA-BH31qHs")]
public class MainApplication : Application
{
	public MainApplication(IntPtr handle, JniHandleOwnership transer)
	  : base(handle, transer)
	{
	}

	public override void OnCreate()
	{
		base.OnCreate();
		CrossCurrentActivity.Current.Init(this);
	}
}