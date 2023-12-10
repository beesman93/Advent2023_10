List<string> lines = new();
using (StreamReader reader = new(args[0]))
{
    while (!reader.EndOfStream)
    {
        lines.Add(reader.ReadLine());
    }
}


List<List<char>> map = new();
List<List<bool>> boundry = new();

int xOrigin=0, yOrigin=0;

for(int i=0;i<lines.Count;i++)
{
    map.Add(new());
    boundry.Add(new());
    for(int j=0;j<lines.Count;j++)
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

boundry[xOrigin][yOrigin] = true;
coord prev = new coord(xOrigin, yOrigin);

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

coord firstPath = new coord(xOrigin, yOrigin);
if (canGoUp)
    firstPath.x--;
else if (canGoDown)
    firstPath.x++;
else if (canGoRight)
    firstPath.y++;
else
    throw new Exception("Start should be able to go to 2 points.");


int pathLen = 0;
while (firstPath.x!= xOrigin || firstPath.y!= yOrigin)
{
    boundry[firstPath.x][firstPath.y] = true;

    char c = map[firstPath.x][firstPath.y];
    coord newCo = moveNext(c, firstPath, prev, map);
    prev = firstPath;
    firstPath = newCo;
    pathLen++;
}

Console.WriteLine(Math.Ceiling(pathLen / 2.0));//part 1

for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[i].Count; j++)
    {
        if (!boundry[i][j])
            map[i][j] = '.';
    }
}

int points_in = 0;
for (int i = 0; i < map.Count; i++)
{
    for (int j = 0; j < map[i].Count; j++)
    {
        if (!boundry[i][j])
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
            }
        }
    }
}
Console.WriteLine(points_in); //part 2
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