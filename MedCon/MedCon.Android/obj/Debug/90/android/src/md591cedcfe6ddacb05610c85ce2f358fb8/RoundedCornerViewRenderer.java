package md591cedcfe6ddacb05610c85ce2f358fb8;


public class RoundedCornerViewRenderer
	extends md51558244f76c53b6aeda52c8a337f2c37.ViewRenderer
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_drawChild:(Landroid/graphics/Canvas;Landroid/view/View;J)Z:GetDrawChild_Landroid_graphics_Canvas_Landroid_view_View_JHandler\n" +
			"";
		mono.android.Runtime.register ("MedCon.Droid.CustomRenderers.RoundedCornerViewRenderer, MedCon.Android", RoundedCornerViewRenderer.class, __md_methods);
	}


	public RoundedCornerViewRenderer (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == RoundedCornerViewRenderer.class)
			mono.android.TypeManager.Activate ("MedCon.Droid.CustomRenderers.RoundedCornerViewRenderer, MedCon.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public RoundedCornerViewRenderer (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == RoundedCornerViewRenderer.class)
			mono.android.TypeManager.Activate ("MedCon.Droid.CustomRenderers.RoundedCornerViewRenderer, MedCon.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public RoundedCornerViewRenderer (android.content.Context p0)
	{
		super (p0);
		if (getClass () == RoundedCornerViewRenderer.class)
			mono.android.TypeManager.Activate ("MedCon.Droid.CustomRenderers.RoundedCornerViewRenderer, MedCon.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public boolean drawChild (android.graphics.Canvas p0, android.view.View p1, long p2)
	{
		return n_drawChild (p0, p1, p2);
	}

	private native boolean n_drawChild (android.graphics.Canvas p0, android.view.View p1, long p2);

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
