package md591cedcfe6ddacb05610c85ce2f358fb8;


public class CustomDatepickerRenderer
	extends md51558244f76c53b6aeda52c8a337f2c37.DatePickerRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"";
		mono.android.Runtime.register ("MedCon.Droid.CustomRenderers.CustomDatepickerRenderer, MedCon.Android", CustomDatepickerRenderer.class, __md_methods);
	}


	public CustomDatepickerRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == CustomDatepickerRenderer.class)
			mono.android.TypeManager.Activate ("MedCon.Droid.CustomRenderers.CustomDatepickerRenderer, MedCon.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public CustomDatepickerRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == CustomDatepickerRenderer.class)
			mono.android.TypeManager.Activate ("MedCon.Droid.CustomRenderers.CustomDatepickerRenderer, MedCon.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public CustomDatepickerRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == CustomDatepickerRenderer.class)
			mono.android.TypeManager.Activate ("MedCon.Droid.CustomRenderers.CustomDatepickerRenderer, MedCon.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}

	private java.util.ArrayList refList;
	public void monodroidAddReference (java.lang.Object obj)
	{
		if (refList == null)
			refList = new java.util.ArrayList ();
		refList.add (obj);
	}

	public void monodroidClearReferences ()
	{
		if (refList != null)
			refList.clear ();
	}
}