

using System.Windows.Forms;
using System.Drawing;
using System.Collections.Generic;
using Ui;
using System.Collections;
using System;
using Ui;
namespace Ctrl
{
    public class New
    {
        public static Label newLabel(string p)
        {
            Label label = new Label();
            label.Text = p;
            label.AutoSize = false;
            label.TextAlign = ContentAlignment.MiddleCenter;
            return label;
        }

        public static ComboBoxClick newComboBox(string text, string[] texts)
        {
            ComboBoxClick label = new ComboBoxClick(text, texts);
            return label;
        }

        public static LabelClick newButton(string p, IClickable iclickable)
        {
            LabelClick label = new LabelClick(new List<IClickable> { iclickable });
            label.Font = new Font("Consol",8, FontStyle.Regular);
            label.Text = p;
            label.AutoSize = false;
            label.TextAlign = ContentAlignment.MiddleCenter;
            label.BackColor = Color.SkyBlue;
            label.BorderStyle = BorderStyle.FixedSingle;
            return label;
        }

        public static TextBox newBox(string p)
        {
            TextBox label = new TextBox();
            label.Text = p;
            label.AutoSize = false;
            label.TextAlign = HorizontalAlignment.Center;
            label.BorderStyle = BorderStyle.FixedSingle;
            label.Multiline = true;
            label.BackColor = Color.LightYellow;
            return label;
        }

        internal static Control newLabel(string p, Color color)
        {
            Control c = newLabel(p);
            c.BackColor = color;
            return c;
        }

        internal static Control newPanel(Color color)
        {
            Control c = new Panel();
            c.BackColor = color;
            return c;
        }
    }

    public abstract class FlowPanel : FlowLayoutPanel , IClickable
    {
        Type _type;
        IClickable _iclickable;

        public FlowPanel(Type type, IClickable iclickable)
        {
            _iclickable = iclickable;
            _type = type;
            FlowDirection = FlowDirection.TopDown;
        }


        public abstract void Inject(IList ilist);

        public void SetSystemItem()
        {
        }

        public abstract void ClickRecieved(string text);
    }

    public class DockCtrl : Panel
    {
        public static DockCtrl CreatePanel(int width, int height)
        {
            DockCtrl bfc = new DockCtrl();
            bfc.Width = width;
            bfc.Height = height;
            return bfc;
        }

        int tabIndex = 1000;

        List<Panel> list = new List<Panel>();
        public Control Top(int height, int margin, Control q)
        {
            Panel p = new Panel();
            p.Height = height;
            p.Dock = DockStyle.Top;
            list.Add(p);
            p.Padding = new Padding(margin, margin, margin, 0);

            q.Dock = DockStyle.Fill;
            p.Controls.Add(q);
            return q;
        }
        public Control Bottum(int height, int margin, Control q)
        {
            Panel p = new Panel();
            p.Height = height;
            p.Dock = DockStyle.Bottom;
            list.Add(p);
            p.Padding = new Padding(margin, 0, margin, margin);

            q.Dock = DockStyle.Fill;
            p.Controls.Add(q);
            return q;
        }
        public Control Left(int width, int margin, Control q)
        {
            Panel p = new Panel();
            p.Width = width;
            p.Dock = DockStyle.Left;
            list.Add(p);
            p.Padding = new Padding(margin, margin, 0, margin);

            q.Dock = DockStyle.Fill;
            p.Controls.Add(q);
            return q;
        }
        public Control Right(int width, int margin, Control q)
        {
            Panel p = new Panel();
            p.Width = width;
            p.Dock = DockStyle.Right;
            list.Add(p);
            p.Padding = new Padding(0, margin, margin, margin);

            q.Dock = DockStyle.Fill;
            p.Controls.Add(q);
            return q;
        }

        public Control Center(int margin, Control q)
        {
            Panel p = new Panel();
            p.Dock = DockStyle.Fill;
            list.Add(p);
            p.Padding = new Padding(margin);

            q.Dock = DockStyle.Fill;
            p.Controls.Add(q);
            return q;
        }


        public void Commit()
        {
            for (int i = list.Count - 1; i >= 0; i--)
            {
                this.Controls.Add(list[i]);
            }
        }

        public void Clear()
        {
            list.Clear();
            this.Controls.Clear();
        }
    }

    public interface IClickable
    {
        void ClickRecieved(string text);
    }

    public class ComboBoxClick : Panel, IClickable
    {
        Panel _inner = new Panel();
        public TextBox TextBox = new TextBox();
        public ComboBoxClick(string text, string[] texts)
        {
            this.Controls.Add(_inner);
            _inner.Dock = DockStyle.Fill;
            _inner.BackColor = Color.Yellow;
            TextBox.Text = text;
            TextBox.ReadOnly = true;
            TextBox.AutoSize = false;
            TextBox.TextAlign = HorizontalAlignment.Center;
            TextBox.BorderStyle = BorderStyle.FixedSingle;
            TextBox.Multiline = true;
            TextBox.BackColor = Color.LightYellow;

            LabelClick left = New.newButton("<<", this);
            LabelClick right = New.newButton(">>", this);
            left.Width = 32;
            left.Dock = DockStyle.Left;
            right.Width = 32;
            right.Left = _inner.Width - 32;
            right.Dock = DockStyle.Right;
            TextBox.Left = 31;
            TextBox.Width = _inner.Width - 31*2;
            right.Height = left.Height = TextBox.Height = _inner.Height;
            left.Anchor = AnchorStyles.Left | AnchorStyles.Bottom | AnchorStyles.Top;
            right.Anchor = AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            TextBox.Anchor = AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom | AnchorStyles.Top;
            _inner.Controls.Add(TextBox);
            _inner.Controls.Add(left);
            _inner.Controls.Add(right);
            Ctrls = texts;
        }

        string[] Ctrls = new string[]{""};
        
        public void Set(string name)
        {
            TextBox.Text = name;
        }

        public void ClickRecieved(string text)
        {
            for (int i = 0; i < Ctrls.Length; i++)
            {
                if (Ctrls[i].Equals(TextBox.Text))
                {
                    if (text.Equals("<<"))
                    {
                        int j = i + Ctrls.Length - 1;
                        Set(Ctrls[j % Ctrls.Length]);
                        return;
                    }
                    if (text.Equals(">>"))
                    {
                        int j = i + 1;
                        Set(Ctrls[j % Ctrls.Length]);
                        return;
                    }
                }
            }
        }
    }

    public class MultiCtrl : Panel , IClickable
    {
        Panel _inner = new Panel();
        public MultiCtrl()
        {
            this.Controls.Add(_inner);
            _inner.Dock = DockStyle.Fill;
        }

        List<Control> Ctrls = new List<Control>();
        public void Set(string name)
        {
            foreach (Control c in Ctrls)
            {
                if (c!=null && c.Name.Equals(name))
                {
                    _inner.Controls.Clear();
                    _inner.Controls.Add(c);
                    c.Dock = DockStyle.Fill;
                    break;
                }
            }
        }

        public void ClickRecieved(string text)
        {
            Set(text);
        }

        internal void Add(Control ctrl, string name)
        {
            Ctrls.Add(ctrl);
            ctrl.Dock = DockStyle.Fill;
            Controls.Add(ctrl);
            if (ctrl != null)
                ctrl.Name = name;
        }
    }

    public class Loop
    {
        public class LoopEvent
        {
            int _counter;
            Action _action;
            public LoopEvent(int counter, Action action)
            {
                _counter = counter;
                _action = action;
            }
            public bool Check(int counter)
            {
                if (counter >= _counter)
                {
                    _action();
                    return true;
                }
                return false;
            }
        }
        List<LoopEvent> LoopEvents = new List<LoopEvent>();

        int _counter;
        Timer _timer = new Timer();
        public Loop()
        {
            _timer.Interval = 50;
            _timer.Start();
            _timer.Tick += new EventHandler(timer_Tick);
        }

        public void Add(int time, Action action)
        {
            LoopEvents.Add(new LoopEvent(time/50+ _counter,action));
        }

        void timer_Tick(object sender, EventArgs e)
        {
            _counter++;
            for (int i = 0; i < LoopEvents.Count; i++)
            {
                if (LoopEvents[i].Check(_counter))
                {
                    LoopEvents.RemoveAt(i);
                    break;
                }
            }
        }
    }

    public class MouseControl
    {
        public delegate void ChangeHandler(int x, int y);
        public event ChangeHandler Change;
        Timer timer = new Timer();

        public MouseControl(Control control, Cursor cursor)
        {
            control.Cursor = cursor;
            control.MouseDown += new MouseEventHandler(c_MouseDown);
            control.MouseUp += new MouseEventHandler(c_MouseUp);
            control.MouseMove += new MouseEventHandler(c_MouseMove);
            timer.Interval = 10;
            timer.Start();
            timer.Tick += new EventHandler(timer_Tick);
        }

        bool _isDown = false;

        void timer_Tick(object sender, EventArgs e)
        {
            if (_isDown)
            {
                int x = (_xDelta - _xDone)/2;
                int y = (_yDelta - _yDone)/2;
                Change(x, y);
                _xDone += x;
                _yDone += y;
            }
        }

        void c_MouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                _xDelta += e.X - _xOld;
                _yDelta += e.Y - _yOld;
            }
        }

        void c_MouseUp(object sender, MouseEventArgs e)
        {
            _isDown = false;
        }

        int _xOld, _yOld;
        int _xDelta, _yDelta;
        int _xDone, _yDone;

        void c_MouseDown(object sender, MouseEventArgs e)
        {
            _xDone = 0;
            _yDone = 0;
            _xDelta = 0;
            _yDelta = 0;
            _xOld = e.X;
            _yOld = e.Y;
            _isDown = true;
        }

        internal void TurnOff()
        {
            _isDown = false;
        }
    }

    public class ChildWin
    {
        UiCreator _page;
        protected Form _form = new Form();
        protected Panel _panel = new Panel();
        bool _isWin = true;
        DockStyle _dock;
        MouseControl _mouseControl;
        public string WindowTitle = "Window";
        Control _windowTitleCtrl;
        object _lock = new object();

        public ChildWin(UiCreator page, string windowTitle)
        {
            WindowTitle = windowTitle;
            _page = page;
            page.FormClosed += new FormClosedEventHandler(page_FormClosed);
            _form.Load += new System.EventHandler(FormTest_Load);
            _form.FormBorderStyle = FormBorderStyle.None;
        }

        void page_FormClosed(object sender, FormClosedEventArgs e)
        {
            _form.DialogResult = _page.DialogResult;
            _form.Close();
            _panel.Parent.Controls.Remove(_panel);
            _panel.Visible = false;
        }

        void FormTest_Load(object sender, System.EventArgs e)
        {
            _page.Dock = DockStyle.Fill;
            _panel.Location = _form.Location;
            _panel.Size = _form.Size;
            this.Replace(_page);
        }

        public void ToolDock(DockStyle dock)
        {
            _mouseControl.TurnOff();
            if (dock != DockStyle.None)
            {
                _form.Visible = false;
                _panel.Visible = true;
                _panel.Dock = dock;
                _dock = dock;
                _isWin = false;
                Replace(_page);
                _windowTitleCtrl.BackColor = Color.Gray;
            }
            else
            {
                _form.Visible = true;
                _panel.Visible = false;
                _isWin = true;
                Replace(_page);
                _windowTitleCtrl.BackColor = Color.FromArgb(255, 0, 200, 0);
            }
        }

        public UiCreator Central;

        internal void Replace(UiCreator c)
        {
            Central = c;
            if (_isWin)
                ReplaceWin(c);
            else
                ReplacePanel(c);
        }


        private void ReplacePanel(Control c)
        {
            _panel.Controls.Clear();
            _panel.Controls.Add(Draw(c));
        }

        
        Control Draw(Control c)
        {
            
            DockCtrl dockCtrl = new DockCtrl();
            if (_dock == DockStyle.Bottom || _dock == DockStyle.None)
            {
                var top = New.newPanel(Color.Green);
                new MouseControl(top, Cursors.SizeNS).Change += new MouseControl.ChangeHandler(Top_Change); ;
                dockCtrl.Top(3, 0, top);
            }
            if (_dock == DockStyle.Right || _dock == DockStyle.None)
            {
                var left = New.newPanel(Color.Green);
                new MouseControl(left, Cursors.SizeWE).Change += new MouseControl.ChangeHandler(Left_Change);
                dockCtrl.Left(3, 0, left);
            }
            if (_dock == DockStyle.Left || _dock == DockStyle.None)
            {
                var right = New.newPanel(Color.Green);
                new MouseControl(right, Cursors.SizeWE).Change += new MouseControl.ChangeHandler(Right_Change);
                dockCtrl.Right(3, 0, right);
            }
            if (_dock == DockStyle.Top || _dock == DockStyle.None)
            {
                var bottom = New.newPanel(Color.Green);
                new MouseControl(bottom, Cursors.SizeNS).Change += new MouseControl.ChangeHandler(Bottom_Change);
                dockCtrl.Bottum(3, 0, bottom);
            }

            _windowTitleCtrl = New.newLabel(WindowTitle, Color.FromArgb(255, 0, 200, 0));
            _mouseControl = new MouseControl(_windowTitleCtrl, Cursors.SizeAll);
            _mouseControl.Change += new MouseControl.ChangeHandler(Title_Change);
            dockCtrl.Top(18, 0, _windowTitleCtrl);

            dockCtrl.Center(0, c);
            dockCtrl.Commit();
            dockCtrl.Dock = DockStyle.Fill;
            return dockCtrl;
        }

        private void ReplaceWin(Control c)
        {
            _form.Controls.Clear();
            _dock = DockStyle.None;
            _form.Controls.Add(Draw(c));
        }


        void Top_Change(int x, int y)
        {
            _form.Top += y;
            _form.Height -= y;
            _panel.Top += y;
            _panel.Height -= y;
        }
        void Bottom_Change(int x, int y)
        {
            _form.Height += y;
            _panel.Height += y;
        }

        void Right_Change(int x, int y)
        {
            _form.Width += x;
            _panel.Width += x;
        }

        void Left_Change(int x, int y)
        {
            _form.Left += x;
            _form.Width -= x;
            _panel.Left += x;
            _panel.Width -= x;
        }

        void Title_Change(int X,int Y)
        {
            _form.Left += X;
            _form.Top += Y;
            if (_form.Left < 0 && _dock == DockStyle.None)
                ToolDock(DockStyle.Left);
            else if (_form.Left > 100 && _dock == DockStyle.Left)
                ToolDock(DockStyle.None);
            else if (_form.Right > ParentWin.Instance().Width && _dock == DockStyle.None)
                ToolDock(DockStyle.Right);
            else if (_form.Right < ParentWin.Instance().Width-100 && _dock == DockStyle.Right)
                ToolDock(DockStyle.None);

            if (_form.Top < 0 && _dock == DockStyle.None)
                ToolDock(DockStyle.Top);
            else if (_form.Top > 100 && _dock == DockStyle.Top)
                ToolDock(DockStyle.None);
            else if (_form.Bottom > ParentWin.Instance().Height && _dock == DockStyle.None)
                ToolDock(DockStyle.Bottom);
            else if (_form.Bottom < ParentWin.Instance().Height - 100 && _dock == DockStyle.Bottom)
                ToolDock(DockStyle.None);

        }

        internal Form GetForm()
        {
            return _form;
        }
    

        public void Resize(int w, int h)
        {
            _form.Width = w;
            _form.Height = h;
        }

        DialogResult _result;

        internal DialogResult Show(bool dialog)
        {
            System.Threading.Thread.Sleep(50);
            _form.MdiParent = ParentWin.Instance();
            _panel.Parent = ParentWin.Instance();
            _panel.Visible = false;
            _form.Show();
            return _form.DialogResult;
        }
    }

    public class ParentWin : Form
    {
        static ParentWin _parentWin;
        
        public static ParentWin Instance()
        {
            if (_parentWin == null)
            {
                _parentWin = new ParentWin();
            }
            return _parentWin;
        }
        
        private ParentWin()
        {
            this.IsMdiContainer = true;
            Background();
        }

        private void Background()
        {
            Form back = new Form();
            back.FormBorderStyle = FormBorderStyle.None;
            back.BackColor = Color.DarkBlue;
            back.Size = Screen.PrimaryScreen.Bounds.Size;
            back.MdiParent = this;
            back.Enabled = false;
            back.Show();
        }
    }

    public class SingelWin : ChildWin
    {
        static Dictionary<string, SingelWin> _win = new Dictionary<string,SingelWin>();

        public SingelWin(UiCreator c,string name):base(c,name)
        {
        }

        public static SingelWin Instance(UiCreator c, string name)
        {
            if (!_win.ContainsKey(name) || _win[name] == null)
            {
                _win[name] = new SingelWin(c, name);
            }
            return _win[name];
        }
        public static SingelWin Instance(string name)
        {
            if (!_win.ContainsKey(name))
            {
                return null;
                if (_win[name] == null)
                {
                    return null;
                }
            }
            return _win[name];
        }
    }

    public class LabelClick : Label
    {
        List<IClickable> _iclicks;
        public LabelClick(List<IClickable> iclicks)
        {
            this.Click += new System.EventHandler(LabelClick_Click);
            _iclicks = iclicks;
        }

        void LabelClick_Click(object sender, System.EventArgs e)
        {
            foreach(IClickable iclick in _iclicks)
            {
                iclick.ClickRecieved(Text);
            }
        }
    }
}