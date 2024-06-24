using System;
using System.Threading.Tasks;
using StackExchange.Redis;

class RedisCRUD
{
    static async Task Main(string[] args)
    {
        var configurationOptions = ConfigurationOptions.Parse("127.0.0.1:6379");
        configurationOptions.AbortOnConnectFail = false;
        configurationOptions.DefaultDatabase = 0;
        ConnectionMultiplexer redis;
        IDatabase redisDb;

        try
        {
            redis = ConnectionMultiplexer.Connect(configurationOptions);
            redisDb = redis.GetDatabase();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Redis Exception: + {ex.Message}\n");
            return;
        }


        while (true)
        {
            var choice = IndexScreen();

            switch (choice)
            {
                case "1":
                    await CrudCreate(redisDb);
                    break;
                case "2":
                    await CrudRead(redisDb);
                    break;
                case "3":
                    await CrudUpdate(redisDb);
                    break;
                case "4":
                    await CrudDelete(redisDb);
                    break;
                case "5":
                    redis.Close();
                    return;
                default:
                    Console.WriteLine("Gecersiz secim.\n");
                    break;
            }
        }
    }

    private static String IndexScreen()
    {
        Console.WriteLine("Redis CRUD islemi seciniz.");
        Console.WriteLine("(1) Create");
        Console.WriteLine("(2) Read");
        Console.WriteLine("(3) Update");
        Console.WriteLine("(4) Delete");
        Console.WriteLine("(5) Exit");
        Console.Write("Secim: ");
        return Console.ReadLine();
    }

    private static async Task CrudCreate(IDatabase redisDb)
    {
        Console.Write("Anahtar (key) giriniz: ");
        var key = Console.ReadLine();
        Console.Write("Deger (value) giriniz: ");
        var value = Console.ReadLine();
        await redisDb.StringSetAsync(key, value);
        Console.WriteLine("Veri basariyla eklendi.\n");
    }

    private static async Task CrudRead(IDatabase redisDb)
    {
        Console.Write("Anahtar (key) giriniz: ");
        var key = Console.ReadLine();
        var value = await redisDb.StringGetAsync(key);
        Console.WriteLine(value.HasValue ? $"Deger (value): {value}\n" : "Veri bulunamadi.\n");
    }

    private static async Task CrudUpdate(IDatabase redisDb)
    {
        Console.Write("Anahtar (key) giriniz: ");
        var key = Console.ReadLine();
        Console.Write("Deger (value) giriniz: ");
        var newValue = Console.ReadLine();
        await redisDb.StringSetAsync(key, newValue);
        Console.WriteLine("Veri basariyla guncellendi.\n");
    }

    private static async Task CrudDelete(IDatabase redisDb)
    {
        Console.Write("Anahtar (key) giriniz: ");
        var key = Console.ReadLine();
        await redisDb.KeyDeleteAsync(key);
        Console.WriteLine("Veri basariyla silindi.\n");
    }
}
