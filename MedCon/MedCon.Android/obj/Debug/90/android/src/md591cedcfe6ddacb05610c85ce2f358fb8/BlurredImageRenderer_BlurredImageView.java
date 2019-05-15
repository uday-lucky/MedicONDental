package md591cedcfe6ddacb05610c85ce2f358fb8;


public class BlurredImageRenderer_BlurredImageView
	extends android.widget.ImageView
	implements
		mono.android.IGCUserPeer
{
/** @hide */
	public static final String __md_methods;
	static {
		__md_methods = 
			"n_invalidate:()V:GetInvalidateHandler\n" +
			"";
		mono.android.Runtime.register ("MedCon.Droid.CustomRenderers.BlurredImageRenderer+BlurredImageView, MedCon.Android", BlurredImageRenderer_BlurredImageView.class, __md_methods);
	}


	public BlurredImageRenderer_BlurredImageView (android.content.Context p0)
	{
		super (p0);
		if (getClass () == BlurredImageRenderer_BlurredImageView.class)
			mono.android.TypeManager.Activate ("MedCon.Droid.CustomRenderers.BlurredImageRenderer+BlurredImageView, MedCon.Android", "Android.Content.Context, Mono.Android", this, new java.lang.Object[] { p0 });
	}


	public BlurredImageRenderer_BlurredImageView (android.content.Context p0, android.util.AttributeSet p1)
	{
		super (p0, p1);
		if (getClass () == BlurredImageRenderer_BlurredImageView.class)
			mono.android.TypeManager.Activate ("MedCon.Droid.CustomRenderers.BlurredImageRenderer+BlurredImageView, MedCon.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android", this, new java.lang.Object[] { p0, p1 });
	}


	public BlurredImageRenderer_BlurredImageView (android.content.Context p0, android.util.AttributeSet p1, int p2)
	{
		super (p0, p1, p2);
		if (getClass () == BlurredImageRenderer_BlurredImageView.class)
			mono.android.TypeManager.Activate ("MedCon.Droid.CustomRenderers.BlurredImageRenderer+BlurredImageView, MedCon.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2 });
	}


	public BlurredImageRenderer_BlurredImageView (android.content.Context p0, android.util.AttributeSet p1, int p2, int p3)
	{
		super (p0, p1, p2, p3);
		if (getClass () == BlurredImageRenderer_BlurredImageView.class)
			mono.android.TypeManager.Activate ("MedCon.Droid.CustomRenderers.BlurredImageRenderer+BlurredImageView, MedCon.Android", "Android.Content.Context, Mono.Android:Android.Util.IAttributeSet, Mono.Android:System.Int32, mscorlib:System.Int32, mscorlib", this, new java.lang.Object[] { p0, p1, p2, p3 });
	}


	public void invalidate ()
	{
		n_invalidate ();
	}

	private native void n_invalidate ();

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
