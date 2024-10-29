using Microsoft.Win32;

public class FileDialogService : IFileDialogService
{
    public string OpenFileDialog()
    {
        OpenFileDialog openFileDialog = new OpenFileDialog
        {
            Filter = "JSON files (*.json)|*.json"
        };
        return openFileDialog.ShowDialog() == true ? openFileDialog.FileName : null;
    }
}
