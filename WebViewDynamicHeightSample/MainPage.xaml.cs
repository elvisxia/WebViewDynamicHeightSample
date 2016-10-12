using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace WebViewDynamicHeightSample
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        WebView webView;
        String hexcolor;
        public MainPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs args)
        {
            hexcolor = "yellow";
            string formItem = "<p><span style=\"font-size:14px;\">To help us investigate your query please can you make a sheet using the following&nbsp;</span><a href=\"http://www.pdf995.com/samples/pdf.pdf\" style=\"font-size: 14px;\" target=\"_blank\">document</a><span style=\"font-size:14px;\">.</span></p><p><strong><span style=\"font-size: 14px;\">Testing WebView Height</span></strong></p>";
            webView = new WebView() { IsHitTestVisible = true };
            string notifyJS = @"<script type='text/javascript' language='javascript'>
                                            function setupBrowser(){
                                            document.touchmove=function(){return false;};;
                                            document.onmousemove=function(){return false;};
                                            document.onselectstart=function(){return false;};
                                            document.ondragstart=function(){return false;}
                                            //window.external.notify('ContentHeight:'+document.body.firstChild.offsetHeight);
                                            window.external.notify('ContentHeight:'+document.getElementById('pageWrapper').offsetHeight);
                                            var links = document.getElementsByTagName('a');
                                            for(var i=0;i<links.length;i++) {
                                               links[i].onclick = function() {
                                                    window.external.notify(this.href);
                                                        return false;
                                            }
                                            }
                                        }
                                    </script>";


            string htmlContent="";
            if (hexcolor != null)
            {
                htmlContent = string.Format("<html><head>{0}</head>" +
                                                "<body onLoad=\"setupBrowser()\" style=\"margin:0px;padding:0px;background-color:{2};\">" +
                                                "<div id=\"pageWrapper\" style=\"width:100%;word-wrap:break-word;padding:0px 25px 0px 25px\">{1}</div></body></html>",
                                                notifyJS,
                                                formItem,
                                                hexcolor);
            }

            webView.NavigateToString(htmlContent);
            rootGrid.Children.Add(webView);

            webView.ScriptNotify += async (sender,e) =>
            {
                if (e.Value.StartsWith("ContentHeight"))
                {
                    (sender as WebView).Height = Convert.ToDouble(e.Value.Split(':')[1]);
                    return;
                }

                if (!string.IsNullOrEmpty(e.Value))
                {
                    string href = e.Value.ToLower();
                    if (href.StartsWith("mailto:"))
                    {
                        LauncherOptions options = new LauncherOptions();
                        options.DisplayApplicationPicker = true;
                        options.TreatAsUntrusted = true;
                        var success = await Launcher.LaunchUriAsync(new Uri(e.Value), options);
                    }
                    else if (href.StartsWith("tel:"))
                    {
                        LauncherOptions options = new LauncherOptions();
                        options.DisplayApplicationPicker = true;
                        options.TreatAsUntrusted = true;
                        var success = await Launcher.LaunchUriAsync(new Uri(e.Value), options);
                    }

                    else
                    {
                        LauncherOptions options = new LauncherOptions();
                        options.DisplayApplicationPicker = true;
                        options.TreatAsUntrusted = true;
                        var success = await Launcher.LaunchUriAsync(new Uri(e.Value), options);
                    }
                }
            };
            base.OnNavigatedTo(args);
        }
    }
}
