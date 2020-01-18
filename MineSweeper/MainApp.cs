using System;

// 지뢰찾기
// 각 필드의 성질들을 비트로 표현하면 훨씬 표현이 쉬울 것 같다.
// 근데 가독성은 내다버렸을 것 같다. 일단 비트를 쓰지 않은 구현이다.

// 고정폭 글꼴을 사용하여 게임을 구동하여야 한다.
// Ex) Consolas, D2Coding(Naver)

namespace Coalery
{
    public class MineSweeper
    {
        private const string prefix = " ";

        private int MAP_SIZE;
        private int MINE_COUNT;
        private int[,] field;
        private bool isGameEnd;

        private int currentSelectX;
        private int currentSelectY;

        private enum FieldType { UNSELECTED = 0, SELECTED = 10, NO_MINE_FLAG = 11, MINE_FLAG = 12, MINE = 13 } // + VALUE TYPE 1 ~ 8

        public MineSweeper() : this(10, 9) { }

        public MineSweeper(int m_MAP_SIZE, int m_MINE_COUNT)
        {
            MAP_SIZE = m_MAP_SIZE;
            MINE_COUNT = m_MINE_COUNT;

            field = new int[MAP_SIZE, MAP_SIZE];

            Random random = new Random();

            for (int i = 0; i < MINE_COUNT; i++)
            {
                int x = random.Next(0, MAP_SIZE);
                int y = random.Next(0, MAP_SIZE);

                if (field[x, y] != 0)
                {
                    i--;
                    continue;
                }

                field[x, y] = (int)FieldType.MINE;
            }
            isGameEnd = false;

            currentSelectX = 0;
            currentSelectY = 0;
        } // Initialize

        public bool IsGameEnd() { return isGameEnd; }

        public bool ValidKey(out int output_key)
        {
            ConsoleKeyInfo key = Console.ReadKey();
            if (key.Key == ConsoleKey.UpArrow && currentSelectY > 0)
                output_key = 1;
            else if (key.Key == ConsoleKey.LeftArrow && currentSelectX > 0)
                output_key = 2;
            else if (key.Key == ConsoleKey.DownArrow && currentSelectY < MAP_SIZE - 1)
                output_key = 3;
            else if (key.Key == ConsoleKey.RightArrow && currentSelectX < MAP_SIZE - 1)
                output_key = 4;
            else if (key.Key == ConsoleKey.D1) // Select Key
                output_key = 5;
            else if (key.Key == ConsoleKey.D2) // Flag Key
                output_key = 6;
            else if (key.Key == ConsoleKey.D0) // Exit Key
                output_key = 7;
            else
            { // If the Input Key is not in arrow, Then Ignore that command.
                output_key = -1;
                return false;
            }
            return true;
        }

        public void IdentifyKey(int key)
        {
            if (key == 1) currentSelectY--;
            else if (key == 2) currentSelectX--;
            else if (key == 3) currentSelectY++;
            else if (key == 4) currentSelectX++;
            else if (key == 5)
            {
                if (field[currentSelectY, currentSelectX] == (int)FieldType.MINE)
                {
                    isGameEnd = true;
                    return;
                }
                Select(currentSelectY, currentSelectX);

                isGameEnd = true;
                for (int i = 0; i < MAP_SIZE; i++)
                    for (int j = 0; j < MAP_SIZE; j++)
                        if (field[i, j] == (int)FieldType.UNSELECTED)
                        {
                            isGameEnd = false;
                            return;
                        }
            }
            else if (key == 6) Flag();
            else if (key == 7) isGameEnd = true;
        }

        public void PrintField()
        {
            Console.Clear();

            for (int i = 0; i < MAP_SIZE; i++)
            {
                for (int j = 0; j < MAP_SIZE; j++)
                {
                    Console.Write(prefix);
                    if (field[i, j] == 999)
                        field[i, j] = (int)FieldType.SELECTED;
                    if (field[i, j] == (int)FieldType.MINE_FLAG || field[i, j] == (int)FieldType.NO_MINE_FLAG)
                        Console.Write("●");
                    //                    else if(field[i, j] == (int)FieldType.MINE) Console.Write("M"); // For DEBUG
                    else if (field[i, j] == (int)FieldType.UNSELECTED || field[i, j] == (int)FieldType.MINE)
                        Console.Write("■");
                    else if (field[i, j] == (int)FieldType.SELECTED)
                        Console.Write("□");
                    else
                        Console.Write(field[i, j]);
                }
                if (i == currentSelectY)
                    Console.Write(" ←");
                Console.WriteLine();
            }
            for (int i = 0; i < 2 * currentSelectX + 1; i++)
                Console.Write(" ");
            Console.WriteLine("↑");
            Console.WriteLine("[1] => Select     [2] => Flag     [0] => Exit");
        }

        private void Select(int x, int y) // 매우 더럽다
        {
            if (field[x, y] != (int)FieldType.UNSELECTED)
                return;

            int check = 0;
            if ((x - 1 >= 0 && y - 1 >= 0) && (field[x - 1, y - 1] == (int)FieldType.MINE || field[x - 1, y - 1] == (int)FieldType.MINE_FLAG)) check++;
            if ((x - 1 >= 0 && y + 1 < MAP_SIZE) && (field[x - 1, y + 1] == (int)FieldType.MINE || field[x - 1, y + 1] == (int)FieldType.MINE_FLAG)) check++;
            if ((x + 1 < MAP_SIZE && y - 1 >= 0) && (field[x + 1, y - 1] == (int)FieldType.MINE || field[x + 1, y - 1] == (int)FieldType.MINE_FLAG)) check++;
            if ((x + 1 < MAP_SIZE && y + 1 < MAP_SIZE) && (field[x + 1, y + 1] == (int)FieldType.MINE || field[x + 1, y + 1] == (int)FieldType.MINE_FLAG)) check++;
            if ((x - 1 >= 0) && (field[x - 1, y] == (int)FieldType.MINE || field[x - 1, y] == (int)FieldType.MINE_FLAG)) check++;
            if ((y - 1 >= 0) && (field[x, y - 1] == (int)FieldType.MINE || field[x, y - 1] == (int)FieldType.MINE_FLAG)) check++;
            if ((x + 1 < MAP_SIZE) && (field[x + 1, y] == (int)FieldType.MINE || field[x + 1, y] == (int)FieldType.MINE_FLAG)) check++;
            if ((y + 1 < MAP_SIZE) && (field[x, y + 1] == (int)FieldType.MINE || field[x, y + 1] == (int)FieldType.MINE_FLAG)) check++;
            field[x, y] = check;

            if (check == 0)
            {
                field[x, y] = 999;
                if (x - 1 >= 0) Select(x - 1, y);
                if (x + 1 < MAP_SIZE) Select(x + 1, y);
                if (y - 1 >= 0) Select(x, y - 1);
                if (y + 1 < MAP_SIZE) Select(x, y + 1);

                if (x - 1 >= 0 && y - 1 >= 0) Select(x - 1, y - 1);
                if (x + 1 < MAP_SIZE && y - 1 >= 0) Select(x + 1, y - 1);
                if (x - 1 >= 0 && y + 1 < MAP_SIZE) Select(x - 1, y + 1);
                if (x + 1 < MAP_SIZE && y + 1 < MAP_SIZE) Select(x + 1, y + 1);
            }
        }

        private void Flag()
        {
            int target = field[currentSelectY, currentSelectX];

            if (target == (int)FieldType.MINE)
                target = (int)FieldType.MINE_FLAG;
            else if (target == (int)FieldType.MINE_FLAG)
                target = (int)FieldType.MINE;
            else if (target == (int)FieldType.UNSELECTED)
                target = (int)FieldType.NO_MINE_FLAG;
            else if (target == (int)FieldType.NO_MINE_FLAG)
                target = (int)FieldType.UNSELECTED;

            field[currentSelectY, currentSelectX] = target;
        }
    }

    public class MainApp
    {
        public static void Main(string[] args)
        {
            MineSweeper mineSweeper = new MineSweeper();

            while (true)
            {
                if (mineSweeper.IsGameEnd())
                    break;
                mineSweeper.PrintField();

                int key = -1;
                if (!mineSweeper.ValidKey(out key))
                    continue;
                mineSweeper.IdentifyKey(key);
            }

        }
    }
}