
namespace HelpProvider2Example
{
    using System;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Drawing;
    using System.Drawing.Design;
    using System.Windows.Forms;
    using Hashtable = System.Collections.Hashtable;
    [ProvideProperty("HelpString", typeof(Control)),
    ProvideProperty("HelpKeyword", typeof(Control)),
    ProvideProperty("HelpNavigator", typeof(Control)),
    ProvideProperty("ShowHelp", typeof(Control)),
    ToolboxItemFilter("System.Windows.Forms")]
    public class HelpProvider2 : Component, IExtenderProvider
    {
        private string helpNamespace = null;
        private Hashtable helpStrings = new Hashtable();
        private Hashtable showHelp = new Hashtable();
        private Hashtable boundControls = new Hashtable();
        private Hashtable keywords = new Hashtable();
        private Hashtable navigators = new Hashtable();
        private object userData;
        public Font Font { get; set; } = SystemFonts.CaptionFont;
        public Color ForeColor { get; set; } = Color.FromKnownColor(KnownColor.WindowText);
        public Color BackColor { get; set; } = Color.FromKnownColor(KnownColor.Window);
        public HelpProvider2() { }

        [Localizable(true),
        DefaultValue(null),
        Editor("System.Windows.Forms.Design.HelpNamespaceEditor, System.Design", typeof(UITypeEditor))]
        public virtual string HelpNamespace
        {
            get => helpNamespace;
            set => this.helpNamespace = value;
        }

        [Localizable(false),
        Bindable(true),
        DefaultValue(null),
        TypeConverter(typeof(StringConverter))]
        public object Tag
        {
            get => userData;
            set => userData = value;
        }

        public virtual bool CanExtend(object target)
        {
            return (target is Control);
        }

        [DefaultValue(null),
        Localizable(true)]
        public virtual string GetHelpKeyword(Control ctl)
        {
            return (string)keywords[ctl];
        }

        [DefaultValue(HelpNavigator.AssociateIndex),
        Localizable(true)]
        public virtual HelpNavigator GetHelpNavigator(Control ctl)
        {
            object nav = navigators[ctl];
            return (nav == null) ? HelpNavigator.AssociateIndex : (HelpNavigator)nav;
        }

        [DefaultValue(null),
        Localizable(true)]
        public virtual string GetHelpString(Control ctl)
        {
            return (string)helpStrings[ctl];
        }

        [Localizable(true)]
        public virtual bool GetShowHelp(Control ctl)
        {
            object b = showHelp[ctl];
            if (b == null)
            {
                return false;
            }
            else
            {
                return (Boolean)b;
            }
        }

        private void OnControlHelp(object sender, HelpEventArgs hevent)
        {
            Control ctl = (Control)sender;
            string helpString = GetHelpString(ctl);
            string keyword = GetHelpKeyword(ctl);
            HelpNavigator navigator = GetHelpNavigator(ctl);
            bool show = GetShowHelp(ctl);

            if (!show)
            {
                return;
            }

            // If the mouse was down, we first try whats this help
            if (Control.MouseButtons != MouseButtons.None && helpString != null)
            {
                if (helpString.Length > 0)
                {
                    HelpExtensions.ShowPopup2(ctl, helpString, hevent.MousePos, Font, BackColor, ForeColor);
                    hevent.Handled = true;
                }
            }

            // If we have a help file, and help keyword we try F1 help next
            if (!hevent.Handled && helpNamespace != null)
            {
                if (keyword != null)
                {
                    if (keyword.Length > 0)
                    {
                        Help.ShowHelp(ctl, helpNamespace, navigator, keyword);
                        hevent.Handled = true;
                    }
                }

                if (!hevent.Handled)
                {
                    Help.ShowHelp(ctl, helpNamespace, navigator);
                    hevent.Handled = true;
                }
            }

            // So at this point we don't have a help keyword, so try to display
            // the whats this help
            //
            if (!hevent.Handled && helpString != null)
            {
                if (helpString.Length > 0)
                {
                    HelpExtensions.ShowPopup2(ctl, helpString, hevent.MousePos, Font, BackColor, ForeColor);
                    hevent.Handled = true;
                }
            }

            // As a last resort, just popup the contents page of the help file...
            //
            if (!hevent.Handled && helpNamespace != null)
            {
                Help.ShowHelp(ctl, helpNamespace);
                hevent.Handled = true;
            }
        }

        private void OnQueryAccessibilityHelp(object sender, QueryAccessibilityHelpEventArgs e)
        {
            Control ctl = (Control)sender;

            e.HelpString = GetHelpString(ctl);
            e.HelpKeyword = GetHelpKeyword(ctl);
            e.HelpNamespace = HelpNamespace;
        }

        public virtual void SetHelpString(Control ctl, string helpString)
        {
            helpStrings[ctl] = helpString;
            if (helpString != null)
            {
                if (helpString.Length > 0)
                {
                    SetShowHelp(ctl, true);
                }
            }
            UpdateEventBinding(ctl);
        }

        public virtual void SetHelpKeyword(Control ctl, string keyword)
        {
            keywords[ctl] = keyword;
            if (keyword != null)
            {
                if (keyword.Length > 0)
                {
                    SetShowHelp(ctl, true);
                }
            }
            UpdateEventBinding(ctl);
        }
        private bool IsEnumValid(Enum enumValue, int value, int minValue, int maxValue)
        {
            bool valid = (value >= minValue) && (value <= maxValue);
            return valid;
        }

        public virtual void SetHelpNavigator(Control ctl, HelpNavigator navigator)
        {
            //valid values are 0x80000001 to 0x80000007
            if (!IsEnumValid(navigator, (int)navigator, (int)HelpNavigator.Topic, (int)HelpNavigator.TopicId))
            {
                //validate the HelpNavigator enum
                throw new InvalidEnumArgumentException("navigator", (int)navigator, typeof(HelpNavigator));
            }

            navigators[ctl] = navigator;
            SetShowHelp(ctl, true);
            UpdateEventBinding(ctl);
        }

        public virtual void SetShowHelp(Control ctl, bool value)
        {
            showHelp[ctl] = value;
            UpdateEventBinding(ctl);
        }

        internal virtual bool ShouldSerializeShowHelp(Control ctl)
        {
            return showHelp.ContainsKey(ctl);
        }

        public virtual void ResetShowHelp(Control ctl)
        {
            showHelp.Remove(ctl);
        }

        private void UpdateEventBinding(Control ctl)
        {
            if (GetShowHelp(ctl) && !boundControls.ContainsKey(ctl))
            {
                ctl.HelpRequested += new HelpEventHandler(this.OnControlHelp);
                ctl.QueryAccessibilityHelp += new QueryAccessibilityHelpEventHandler(this.OnQueryAccessibilityHelp);
                boundControls[ctl] = ctl;
            }
            else if (!GetShowHelp(ctl) && boundControls.ContainsKey(ctl))
            {
                ctl.HelpRequested -= new HelpEventHandler(this.OnControlHelp);
                ctl.QueryAccessibilityHelp -= new QueryAccessibilityHelpEventHandler(this.OnQueryAccessibilityHelp);
                boundControls.Remove(ctl);
            }
        }

        public override string ToString()
        {
            string s = base.ToString();
            return s + ", HelpNamespace: " + HelpNamespace;
        }
    }
}
