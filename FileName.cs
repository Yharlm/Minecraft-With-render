//int[,] grid = new int[1000, 1000];
//int[,] camera = new int[15, 31];
//grid[12, 37] = 1;
//int offset_x = 0;
//int offset_y = 0;
//while (true)
//{



//    for (int i = 0; i < camera.GetLength(0); i++)
//    {
//        for (int j = 0; j < camera.GetLength(1); j++)
//        {
//            if (grid[i + offset_y, j + offset_x] == 0)
//            {
//                WriteAt("██", j * 2, i);
//            }
//            else
//            {
//                WriteAt("  ", j * 2, i);
//            }
//        }
//    }
//    string input = Console.ReadKey().Key.ToString();
//    switch (input)
//    {
//        case "A":
//            offset_x--;
//            break;
//        case "D":
//            offset_x++;
//            break;
//        case "W":
//            offset_y--;
//            break;
//        case "S":
//            offset_y++;
//            break;

//    }
//}