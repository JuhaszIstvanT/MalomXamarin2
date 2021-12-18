using MalomXamarin2.Persistence;
using MalomXamarin2.Model;
using System;
using System.IO;
using System.Linq;
using Xamarin.Forms;
using MalomXamarin2.Droid.Persistence;

[assembly: Dependency(typeof(AndroidDataAccess))]
namespace MalomXamarin2.Droid.Persistence
{
    public class AndroidDataAccess : IMalomDataAccess
    {
        public MalomBoard Load(String path)
        {
            string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);

            try
            {
                using (StreamReader reader = new StreamReader(filePath))
                {
                    MalomBoard board = new MalomBoard();
                    String line = reader.ReadLine();
                    line = line.Remove(line.Length - 1);
                    String[] nums = line.Split(' ');
                    Player[] p = nums.Select(num => (Player)int.Parse(num)).ToArray();
                    for (int i = 0; i < 3; ++i)
                        for (int j = 0; j < 8; ++j)
                            board[i, j] = p[i * 8 + j];

                    line = reader.ReadLine();
                    line = line.Remove(line.Length - 1);
                    nums = line.Split(' ');
                    Player[] pM = nums.Select(p => (Player)int.Parse(p)).ToArray();
                    line = reader.ReadLine();
                    line = line.Remove(line.Length - 1);
                    nums = line.Split(' ');
                    Player[] cM = nums.Select(c => (Player)int.Parse(c)).ToArray();
                    line = reader.ReadLine();
                    board.steps = int.Parse(line);

                    for (int i = 0; i < 16; ++i)
                    {
                        board.prevMalom[i] = pM[i];
                        board.currentMalom[i] = cM[i];
                    }
                    return board;
                }
            }
            catch
            {
                throw new MalomDataException("Error occured during reading.");
            }
        }

        public void Save(string path, MalomBoard board)
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(path))
                {
                    for (int i = 0; i < 3; ++i)
                        for (int j = 0; j < 8; ++j)
                            writer.Write((int)board[i, j] + " ");
                    writer.WriteLine();

                    for (int i = 0; i < 16; ++i)
                        writer.Write((int)board.prevMalom[i] + " ");
                    writer.WriteLine();

                    for (int i = 0; i < 16; ++i)
                        writer.Write((int)board.currentMalom[i] + " ");
                    writer.WriteLine();
                    writer.Write((int)board.steps);

                    string text = writer.ToString();
                    string filePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), path);

                    File.WriteAllText(filePath, text);
                }
            }
            catch
            {
                throw new MalomDataException("Error occured during writing.");
            }
        }
    }
}