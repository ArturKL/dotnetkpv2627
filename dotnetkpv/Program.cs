string Reverse(string s)
{
    return string.Join("", s.Reverse());
}

// без знаков препинания
string ReverseSentence(string s)
{
    return string.Join(" ", s.Split().Select(Reverse));
}

Console.WriteLine(Reverse("Hello")); // olleH
Console.WriteLine(ReverseSentence("Hello World")); // olleH dlroW