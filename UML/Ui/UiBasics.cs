

using Ctrl;
using System.Windows.Forms;
using System.Collections.Generic;
using System;
using System.Reflection;
using System.Collections;
using System.Drawing;
namespace Ui
{
    public abstract class UiHelper
    {
        public static TextBox AddName(string labelName, DockCtrl d, string name)
        {
            DockCtrl q = new DockCtrl();
            d.Top(25, 2, q);
            Label label = New.newLabel(labelName);
            TextBox text = New.newBox(name);
            q.Left(64, 2, label);
            q.Center(2, text);
            q.Commit();
            return text;
        }
        public static TextBox AddCombo(string name, DockCtrl d, string item, string[] items)
        {
            DockCtrl q = new DockCtrl();
            d.Top(25, 2, q);
            Label label = New.newLabel(name);
            ComboBoxClick text = New.newComboBox(item, items);
            q.Left(64, 2, label);
            q.Center(2, text);
            q.Commit();
            return text.TextBox;
        }

        public static Control AddTab(DockCtrl tabs, MultiCtrl multi, Control ctrl, string name, Color color, int w)
        {
            var labelClick = New.newButton(name, multi);
            labelClick.BackColor = color;
            tabs.Left(w, 2, labelClick);
            multi.Add(ctrl, name);
            return ctrl;
        }
        public static Control AddTab(DockCtrl tabs, IClickable multi, string name)
        {
            Control c = New.newButton(name, multi);
            tabs.Left(30, 2, c);
            return c;

        }
    }

    public abstract class UiCreator : DockCtrl
    {

        public DialogResult DialogResult;

        public event FormClosedEventHandler FormClosed;

        protected void Close(CloseReason closeReason, DialogResult dialogResult)
        {
            DialogResult = dialogResult;
            FormClosed(this, new FormClosedEventArgs(closeReason));
        }

        protected object _obj;
        Dictionary<FieldInfo, Control> _ctrlList = new Dictionary<FieldInfo, Control>();
        public UiCreator(object obj)
        {
            _obj = obj;
        }
        public UiCreator()
        {
        }
        public Control Get(FieldInfo name)
        {
            return _ctrlList[name];
        }
        public void SetFieldCtrl(FieldInfo name, Control ctrl)
        {
            _ctrlList.Add(name, ctrl);
        }
        public string GetValue(FieldInfo name)
        {
            object get = Get(name);
            if(get is TextBox)
                return (get as TextBox).Text;
            return null;
        }
        public void Save(FieldInfo name)
        {
            name.SetValue( _obj, Get(name));
        }
        public void Save()
        {
            foreach (KeyValuePair<FieldInfo, Control> i in _ctrlList)
            {
                i.Key.SetValue(_obj, GetValue(i.Key));
            }
        }

        public abstract void Begin();
        public abstract void Body();
        public abstract void End();
        public abstract void Create(params object[] values);
    }
}
