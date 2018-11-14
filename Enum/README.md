# Smart enums / Type-safe enums pattern
_______________________________________________________________________________

### Шаблон перечисления type-safe: ###
```c#
// шаблон перечисления type-safe
public sealed class Role
{
    public static Role Guest         { get; } = new Role(0, "Читатель", "может читать и комментировать");
    public static Role Editor        { get; } = new Role(1, "Редактор", "имеет доступ ко всем страницам, сообщениям, комментариям");
    public static Role Author        { get; } = new Role(2, "Автор", "может публиковать загруженные фотографии, писать и редактировать собственные сообщения");
    public static Role Contributor   { get; } = new Role(3, "Сотрудник", "может писать и редактировать собственные сообщения");
    public static Role Administrator { get; } = new Role(4, "Администратор", "имеет полный доступ к сайту");

    private Role(int id, string name, string description)
    {
        Id          = id;
        Name        = name;
        Description = description;
    }

    public int    Id          { get; }
    public string Name        { get; }
    public string Description { get; }
}
```
### Некоторые функциональные возможности, как у перечисления: ###
```c#
public override string ToString() => Name;
public static IEnumerable<string> GetNames() => GetValues().Select(role => role.Name);

public static explicit operator Role(int id)   => GetValue(id); // Role role = (Role)1;
public static explicit operator int(Role role) => role.Id;    // int value = (int)Role.Author;

public static Role GetValue(int id)      => GetValues().First(role => role.Id   == id);
public static Role GetValue(string name) => GetValues().First(role => role.Name == name);

public static IReadOnlyList<Role> GetValues()
{
    // либо заполненить коллекцию в конструкторе
    return typeof(Role).GetProperties(
        BindingFlags.Public | BindingFlags.Static
    ).Select(property => (Role)property.GetValue(null)).ToList();
}
```
### Тест: ###
```c#
class Program
{
    static void Main()
    {
        var role = testEnums(Role.Editor);
        System.Console.WriteLine(role);
        // Редактор имеет доступ ко всем страницам, сообщениям, комментариям
        string testEnums(Role value)
        {
            switch (value)
            {
                case var _ when value == Role.Guest:
                    return $"{Role.Guest} {Role.Guest.Description}";

                case var _ when value == Role.Editor:
                    return $"{Role.Editor} {Role.Editor.Description}";
            }
            return "no matches found!";
        }
    }
}
```
