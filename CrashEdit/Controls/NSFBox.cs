using CrashEdit.Crash;
using System.Drawing;
using System.Windows.Forms;
using System.Collections.Generic;

namespace CrashEdit.CE
{
    public sealed class NSFBox : MainControl
    {
        private static ImageList imglist;

        static NSFBox()
        {
            imglist = new ImageList { ColorDepth = ColorDepth.Depth32Bit };
            try
            {
                imglist.Images.Add("default",OldResources.FileImage);
                imglist.Images.Add("nsf",new Icon(OldResources.NSFIcon,16,16));
                imglist.Images.Add("yellowj",OldResources.YellowJournalImage);
                imglist.Images.Add("image",OldResources.ImageImage);
                imglist.Images.Add("bluej",OldResources.BlueJournalImage);
                imglist.Images.Add("music",OldResources.MusicImage);
                imglist.Images.Add("musicred",OldResources.MusicRedImage);
                imglist.Images.Add("musicyellow",OldResources.MusicYellowImage);
                imglist.Images.Add("whitej",OldResources.WhiteJournalImage);
                imglist.Images.Add("thing",OldResources.ThingImage);
                imglist.Images.Add("speaker",OldResources.SpeakerImage);
                imglist.Images.Add("arrow",OldResources.ArrowImage);
                imglist.Images.Add("greyb",OldResources.GreyBuckle);
                imglist.Images.Add("codeb",OldResources.CodeBuckle);
                imglist.Images.Add("crimsonb",OldResources.CrimsonBuckle);
                imglist.Images.Add("limeb",OldResources.LimeBuckle);
                imglist.Images.Add("blueb",OldResources.BlueBuckle);
                imglist.Images.Add("violetb",OldResources.VioletBuckle);
                imglist.Images.Add("redb",OldResources.RedBuckle);
                imglist.Images.Add("yellowb",OldResources.YellowBuckle);
            }
            catch
            {
                imglist.Images.Clear();
            }
        }

        public NSFBox(NSF nsf, GameVersion gameversion) : base(new NSFController(nsf, gameversion).Modern)
        {
            NSF = nsf;
            NSFController = (NSFController)RootController.Legacy;

            RootController.Sync();
            Sync();

            ResourceTree.ImageList = imglist;
        }

        public NSF NSF { get; }
        public NSFController NSFController { get; }
    }
}
