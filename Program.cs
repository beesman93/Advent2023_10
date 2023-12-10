List<string> lines = new();
using (StreamReader reader = new(args[0]))
{
    while (!reader.EndOfStream)
    {
        lines.Add(reader.ReadLine());
    }
}


List<List<char>> map = new();
List<List<int>> boundry = new();

int x=0, y=0;

foreach (string line in lines)
{
    map.Add(new());
    boundry.Add(new());
    foreach (char c in line)
    {
        map.Last().Add(c);
        boundry.Last().Add(0);
    }
}

for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[i].Count; j++)
    {
        if (map[i][j] == 'S')
        {
            x = i;
            y = j;
        }
    }
}

int max_x = 0;
int max_y = 0;

int min_x = int.MaxValue;
int min_y = int.MaxValue;

coord prev = new coord(x,y);

boundry[x][y] = 1;

coord firstPath = new coord(x+1, y);

int pathLen = 0;
while (firstPath.x!=x||firstPath.y!=y)
{
    boundry[firstPath.x][firstPath.y] = 1;

    char c = map[firstPath.x][firstPath.y];
    coord newCo = moveNext(c, firstPath, prev, map);
    prev = firstPath;
    firstPath = newCo;
    pathLen++;

    if (firstPath.x > max_x) max_x = firstPath.x;
    if (firstPath.x < min_x) min_x = firstPath.x;
    if (firstPath.y > max_y) max_y = firstPath.y;
    if (firstPath.y < min_y) min_y = firstPath.y;

}

Console.WriteLine(Math.Ceiling(pathLen / 2.0));

map[x][y] = '7';

for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[i].Count; j++)
    {
        if (boundry[i][j] == 0)
            map[i][j] = '.';
    }
}

/*
for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[i].Count; j++)
    {
        Console.Write(map[i][j]);
    }
    Console.WriteLine();
}
Console.WriteLine();
Console.WriteLine();*/

int points_in = 0;
for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[i].Count; j++)
    {
        if (boundry[i][j] == 0)
        {
            int count_intersects_right = 0;

            int jj = j;
            while (jj < map[i].Count)
            {
                if (map[i][jj] == '|')
                    count_intersects_right++;
                if (map[i][jj] == 'F')
                {
                    int jjj = jj;
                    bool isIntersect = false;
                    while (jjj < map[i].Count)
                    {
                        if (map[i][jjj] == '7')
                            break;
                        if (map[i][jjj] == 'J')
                        {
                            isIntersect = true;
                            break;
                        }
                        jjj++;
                    }
                    if(isIntersect)
                        count_intersects_right++;
                }
                if (map[i][jj] == 'L')
                {
                    int jjj = jj;
                    bool isIntersect = false;
                    while (jjj < map[i].Count)
                    {
                        if (map[i][jjj] == 'J')
                            break;
                        if (map[i][jjj] == '7')
                        {
                            isIntersect = true;
                            break;
                        }
                        jjj++;
                    }
                    if (isIntersect)
                        count_intersects_right++;
                }
                jj++;
            }

            if (count_intersects_right % 2 == 1)
            {
                points_in++;
                //Console.WriteLine($"{i},{j}");
            }
        }
    }
}
Console.WriteLine(points_in);



/*
 * | is a vertical pipe connecting north and south.
- is a horizontal pipe connecting east and west.
L is a 90-degree bend connecting north and east.
J is a 90-degree bend connecting north and west.
7 is a 90-degree bend connecting south and west.
F is a 90-degree bend connecting south and east.
. is ground; there is no pipe in this tile.
S is the starting position of the animal; there is a pipe on this tile, but your sketch doesn't show what shape the pipe has.
*/


coord moveNext(char c, coord curr, coord prev, List<List<char>> map)
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