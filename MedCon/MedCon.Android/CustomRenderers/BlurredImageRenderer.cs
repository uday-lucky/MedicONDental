﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using MedCon.CustomControls;
using MedCon.Droid.CustomRenderers;
using Xamarin.Forms.Platform.Android;
using Android.Graphics.Drawables;
using Android.Graphics;
using System.Threading.Tasks;
using System.IO;
using Android.Renderscripts;
using System.Reflection;

[assembly:ExportRenderer(typeof(BlurredImage),typeof(BlurredImageRenderer))]
namespace MedCon.Droid.CustomRenderers
{
    public class BlurredImageRenderer : ViewRenderer<BlurredImage, ImageView>
    {
        private bool _isDisposed;

        public BlurredImageRenderer()
        {
            AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<BlurredImage> e)
        {
            base.OnElementChanged(e);

            if (Control == null)
            {
                var imageView = new BlurredImageView(Context);
                SetNativeControl(imageView);
            }

            UpdateBitmap(e.OldElement);
            UpdateAspect();
        }

        protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == Image.SourceProperty.PropertyName)
            {
                UpdateBitmap(null);
                return;
            }
            if (e.PropertyName == Image.AspectProperty.PropertyName)
            {
                UpdateAspect();
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (_isDisposed)
            {
                return;
            }
            _isDisposed = true;
            BitmapDrawable bitmapDrawable;
            if (disposing && Control != null && (bitmapDrawable = (Control.Drawable as BitmapDrawable)) != null)
            {
                var bitmap = bitmapDrawable.Bitmap;
                if (bitmap != null)
                {
                    bitmap.Recycle();
                    bitmap.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        private void UpdateAspect()
        {
            using (var scaleType = ToScaleType(Element.Aspect))
            {
                Control.SetScaleType(scaleType);
            }
        }

        private static ImageView.ScaleType ToScaleType(Aspect aspect)
        {
            switch (aspect)
            {
                case Aspect.AspectFill:
                    return ImageView.ScaleType.CenterCrop;
                case Aspect.Fill:
                    return ImageView.ScaleType.FitXy;
            }
            return ImageView.ScaleType.FitCenter;
        }

        private async void UpdateBitmap(Image previous = null)
        {
            Bitmap bitmap = null;
            var source = Element.Source;
            if (previous == null || !object.Equals(previous.Source, Element.Source))
            {
                SetIsLoading(true);
                ((BlurredImageView)base.Control).SkipInvalidate();

                // I’m not sure where this comes from.
                Control.SetImageResource(17170445);
                if (source != null)
                {
                    try
                    {
                        bitmap = await GetImageFromImageSource(source, Context);
                    }
                    catch (TaskCanceledException)
                    {
                    }
                    catch (IOException)
                    {
                    }
                    catch (NotImplementedException)
                    {
                    }
                }
                if (Element != null && object.Equals(Element.Source, source))
                {
                    if (!_isDisposed)
                    {
                        Control.SetImageBitmap(bitmap);
                        bitmap?.Dispose();
                        SetIsLoading(false);
                        ((IVisualElementController)base.Element).NativeSizeChanged();
                    }
                }
            }
        }

        private async Task<Bitmap> GetImageFromImageSource(ImageSource imageSource, Context context)
        {
            IImageSourceHandler handler;

            if (imageSource is FileImageSource)
            {
                handler = new FileImageSourceHandler();
            }
            else if (imageSource is StreamImageSource)
            {
                handler = new StreamImagesourceHandler(); // sic
            }
            else if (imageSource is UriImageSource)
            {
                handler = new ImageLoaderSourceHandler(); // sic
            }
            else
            {
                throw new NotImplementedException();
            }

            var originalBitmap = await handler.LoadImageAsync(imageSource, context);

            var blurredBitmap = await Task.Run(() => CreateBlurredImage(originalBitmap, 25));

            return blurredBitmap;
        }

        private Bitmap CreateBlurredImage(Bitmap originalBitmap, int radius)
        {
            // Create another bitmap that will hold the results of the filter.
            Bitmap blurredBitmap;
            blurredBitmap = Bitmap.CreateBitmap(originalBitmap);

            // Create the Renderscript instance that will do the work.
            var rs = RenderScript.Create(Context);

            // Allocate memory for Renderscript to work with
            var input = Allocation.CreateFromBitmap(rs, originalBitmap, Allocation.MipmapControl.MipmapFull, AllocationUsage.Script);
            var output = Allocation.CreateTyped(rs, input.Type);

            // Load up an instance of the specific script that we want to use.
            var script = ScriptIntrinsicBlur.Create(rs, Android.Renderscripts.Element.U8_4(rs));
            script.SetInput(input);

            // Set the blur radius
            script.SetRadius(radius);

            // Start Renderscript working.
            script.ForEach(output);

            // Copy the output to the blurred bitmap
            output.CopyTo(blurredBitmap);

            return blurredBitmap;
        }

        private class BlurredImageView : ImageView
        {
            private bool _skipInvalidate;

            public BlurredImageView(Context context) : base(context)
            {
            }

            public override void Invalidate()
            {
                if (this._skipInvalidate)
                {
                    this._skipInvalidate = false;
                    return;
                }
                base.Invalidate();
            }

            public void SkipInvalidate()
            {
                this._skipInvalidate = true;
            }
        }

        private static FieldInfo _isLoadingPropertyKeyFieldInfo;

        private static FieldInfo IsLoadingPropertyKeyFieldInfo => _isLoadingPropertyKeyFieldInfo ??
        (_isLoadingPropertyKeyFieldInfo =
        typeof(Image).GetField("IsLoadingPropertyKey", BindingFlags.Static | BindingFlags.NonPublic));

        private void SetIsLoading(bool value)
        {
            var fieldInfo = IsLoadingPropertyKeyFieldInfo;
            ((IElementController)base.Element).SetValueFromRenderer((BindablePropertyKey)fieldInfo.GetValue(null), value);
        }
    }
}