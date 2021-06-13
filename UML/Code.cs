


using System.Windows.Forms;
using Ctrl;
using Ui;


class UiExample : UiCreator
{
    public override void Begin()
    {
    }
    public override void Body()
    {
    }
    public override void End()
    {
    }

    public override void Create(params object[] values)
    {
    }

}

class AppContext : ApplicationContext
{
    static public Loop Loop = new Loop();

    public AppContext()
    {
        ParentWin parent = ParentWin.Instance();
        parent.Show();
        parent.FormClosed += new FormClosedEventHandler(parent_FormClosed);


        var win = SingelWin.Instance(new UiExample(), "Explorer");
        win.Resize((Screen.PrimaryScreen.Bounds.Width * 3) / 4, (Screen.PrimaryScreen.Bounds.Height * 3) / 4);
        win.Show(false);

        var pro = SingelWin.Instance(new UiExample(), "Properties");
        pro.Resize(100, 100);
        pro.Show(false);
        pro.ToolDock(DockStyle.Right);
    }

    void parent_FormClosed(object sender, FormClosedEventArgs e)
    {
        ExitThread();
    }
}
