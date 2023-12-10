using System.Collections.Generic;
using System.Diagnostics;

Stopwatch sw = Stopwatch.StartNew();

Console.Clear();
Thread.Sleep(10);

List<string> lines = new();
using (StreamReader reader = new(args[0]))
{
    while (!reader.EndOfStream)
    {
        lines.Add(reader.ReadLine());
    }
}
long readInputTime = sw.ElapsedMilliseconds;
//Console.WriteLine($"readInput in {readInputTime} ms");

List<List<char>> map = new();
List<List<bool>> boundry = new();

int xOrigin=0, yOrigin=0;
int pathLen = 0;
int points_in = 0;

for (int i=0;i<lines.Count;i++)
{
    map.Add(new());
    boundry.Add(new());
    for(int j = 0; j < lines[i].Length;j++)
    { 
        map.Last().Add(lines[i][j]);
        boundry.Last().Add(false);      
        if (map[i][j] == 'S')
        {
            xOrigin = i;
            yOrigin = j;
        }
    }
}

//drawMap(map,boundry,pathLen,points_in);

boundry[xOrigin][yOrigin] = true;
coord prev = new(xOrigin, yOrigin);

drawMap(map, boundry, pathLen, points_in);

bool canGoUp, canGoDown, canGoLeft, canGoRight;
canGoUp=canGoDown=canGoLeft=canGoRight=false;

if (xOrigin - 1 > 0 &&(
    map[xOrigin - 1][yOrigin]=='|' ||
    map[xOrigin - 1][yOrigin] == 'F' ||
    map[xOrigin - 1][yOrigin] == '7')
    ) canGoUp=true;
if (xOrigin + 1 < map.Count && (
    map[xOrigin + 1][yOrigin] == '|' ||
    map[xOrigin + 1][yOrigin] == 'L' ||
    map[xOrigin + 1][yOrigin] == 'J')
    ) canGoDown = true;
if (yOrigin - 1 > 0 && (
    map[xOrigin][yOrigin - 1] == '-' ||
    map[xOrigin][yOrigin - 1] == 'F' ||
    map[xOrigin][yOrigin - 1] == 'L')
    ) canGoLeft = true;
if (yOrigin + 1 < map[0].Count && (
    map[xOrigin][yOrigin + 1] == '-' ||
    map[xOrigin][yOrigin + 1] == '7' ||
    map[xOrigin][yOrigin + 1] == 'J')
    ) canGoRight = true;

if (canGoUp && canGoDown) map[xOrigin][yOrigin] = '|';
if (canGoUp && canGoLeft) map[xOrigin][yOrigin] = 'J';
if (canGoUp && canGoRight) map[xOrigin][yOrigin] = 'L';
if (canGoDown && canGoLeft) map[xOrigin][yOrigin] = '7';
if (canGoDown && canGoRight) map[xOrigin][yOrigin] = 'F';
if (canGoLeft && canGoRight) map[xOrigin][yOrigin] = '-';

coord firstPath = new(xOrigin, yOrigin);
if (canGoUp)
    firstPath.x--;
else if (canGoDown)
    firstPath.x++;
else if (canGoRight)
    firstPath.y++;
else
    throw new Exception("Start should be able to go to 2 points.");

//drawMap(map, boundry, pathLen, points_in);


while (firstPath.x!= xOrigin || firstPath.y!= yOrigin)
{
    boundry[firstPath.x][firstPath.y] = true;
    mapAns(pathLen,points_in);
    drawMapXY(firstPath.x,firstPath.y,map, boundry, pathLen, points_in);
    char c = map[firstPath.x][firstPath.y];
    coord newCo = moveNext(c, firstPath, prev, map);
    prev = firstPath;
    firstPath = newCo;
    pathLen++;
}

//drawMap(map, boundry, pathLen, points_in);

pathLen = (int)Math.Ceiling(pathLen / 2.0);

drawMap(map, boundry, pathLen, points_in);

for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[i].Count; j++)
    {
        if (!boundry[i][j])
            map[i][j] = ' ';
        //drawMapXY(i, j, map, boundry, pathLen, points_in);
    }
}

drawMap(map, boundry, pathLen, points_in);

for (int i=0; i < map.Count; i++)
{
    for (int j = 0; j < map[i].Count; j++)
    {
        if (!boundry[i][j])
        {
            map[i][j] = '0';
            //drawMapXY(i, j, map, boundry, pathLen, points_in);
            int count_intersects_right = 0;
            
            int jj = j;
            while (jj < map[i].Count)
            {
                if (map[i][jj] == '|')
                    count_intersects_right++;
                if (map[i][jj] == 'F' || map[i][jj] == 'L')
                {
                    int jjj = jj;
                    bool isIntersect = false;
                    while (jjj < map[i].Count)
                    {
                        if (map[i][jjj] == '7')
                        {
                            isIntersect = map[i][jj] == 'L';
                            break;
                        }
                        if (map[i][jjj] == 'J')
                        {
                            isIntersect = map[i][jj] == 'F';
                            break;
                        }
                        jjj++;
                    }
                    if(isIntersect)
                        count_intersects_right++;
                }
                
                jj++;
                map[i][j] = (char)((count_intersects_right % 2)+48);
                
            }
            //drawMapXY(i, j, map, boundry, pathLen, points_in);
            
            if (count_intersects_right % 2 == 1)
            {
                points_in++;
                mapAns(pathLen, points_in);
            }
            drawMapXY(i, j, map, boundry, pathLen, points_in);
        }
    }
}

//drawMap(map, boundry, pathLen, points_in);

//Console.WriteLine(points_in); //part 2

//Console.WriteLine($"solutions in {sw.ElapsedMilliseconds-readInputTime} ms ({sw.ElapsedMilliseconds} ms total)");


static void drawMapXY(int x, int y, List<List<char>> map, List<List<bool>> boundry,int ans_1,int ans_2)
{
    Console.SetCursorPosition(y,x+4);

    if (boundry[x][y])
        Console.ForegroundColor = ConsoleColor.Blue;
    else if (map[x][y] >= '0' && map[x][y] <= '2')
    {
        if (Convert.ToInt32(map[x][y]) % 2 == 1)
            Console.ForegroundColor = ConsoleColor.Green;
        else
            Console.ForegroundColor = ConsoleColor.Red;
    }
    else
        Console.ForegroundColor = ConsoleColor.White;
    char c = map[x][y] switch
    {
        '|' => '│',
        '-' => '─',
        'L' => '└',
        'J' => '┘',
        '7' => '┐',
        'F' => '┌',
        '0' => '0',
        '1' => '1',
        _ => map[x][y],
    };
    Console.Write(c);
    Console.ForegroundColor = ConsoleColor.White;

}

static void mapAns(int ans_1, int ans_2)
{
    Console.SetCursorPosition(0, 0);

    Console.WriteLine();
    Console.WriteLine($"solution1: {ans_1}   ");
    Console.WriteLine($"solution2: {ans_2}   ");
    Console.WriteLine();
}

static void drawMap(List<List<char>> map, List<List<bool>> boundry, int ans_1, int ans_2)
{
    Console.SetCursorPosition(0, 0);

    Console.WriteLine();
    Console.WriteLine($"solution1: {ans_1}   ");
    Console.WriteLine($"solution2: {ans_2}   ");
    Console.WriteLine();

    for (int x = 0; x < map.Count; x++)
    {
        for (int y = 0; y < map[x].Count; y++)
        {
            if (boundry[x][y])
                Console.ForegroundColor = ConsoleColor.Blue;
            else if (map[x][y] >= '0' && map[x][y] <= '2')
            {
                if (Convert.ToInt32(map[x][y]) % 2 == 1)
                    Console.ForegroundColor = ConsoleColor.Green;
                else
                    Console.ForegroundColor = ConsoleColor.Red;
            }
            else
                Console.ForegroundColor = ConsoleColor.White;
            char c = map[x][y] switch
            {
                '|' => '│',
                '-' => '─',
                'L' => '└',
                'J' => '┘',
                '7' => '┐',
                'F' => '┌',
                '0' => '0',
                '1' => '1',
                _ => map[x][y],
            };
            Console.Write(c);
        }
        Console.WriteLine();
    }

    Thread.Sleep(10);
}

static coord moveNext(char c, coord curr, coord prev, List<List<char>> map)
{
    if(c == '|')
    {
        if (prev.x > curr.x)
            return new coord(curr.x - 1, curr.y);
        return new coord(curr.x + 1, curr.y);
    }
    if (c == '-')
    {
        if (prev.y > curr.y)
            return new coord(curr.x, curr.y-1);
        return new coord(curr.x, curr.y+1);
    }
    if (c == 'L')
    {
        if (prev.x < curr.x)
            return new coord(curr.x, curr.y + 1);
        return new coord(curr.x-1, curr.y);
    }
    if (c == 'J')
    {
        if (prev.x < curr.x)
            return new coord(curr.x, curr.y - 1);
        return new coord(curr.x -1 , curr.y);
    }
    if (c == '7')
    {
        if (prev.x > curr.x)
            return new coord(curr.x, curr.y - 1);
        return new coord(curr.x+1, curr.y);
    }
    if (c == 'F')
    {
        if (prev.x > curr.x)
            return new coord(curr.x, curr.y + 1);
        return new coord(curr.x+1, curr.y);
    }
    if (c == 'S')
    {
        return curr;
    }
    if (c == '.')
    {
        throw new Exception("done goofed");
    }

    return new coord(0, 0);
}

struct coord
{
    public int x, y;
    public coord(int x, int y)
    { 
        this.x = x; this.y = y;
    }
}