🛡️ Active Directory Dynamic Mapper

Active Directory üzerinde dinamik olarak kullanıcı ve grup bilgilerini sorgulayıp, istenen property'lerle eşleşen sonuçları kolayca çekebileceğiniz hafif ve hızlı bir .NET API projesi.


🧩 Request nesnesi

var request = new RequestItem
{
    [Required]
    LdapServer = "your-ldap-server",

    [Optional]
    UserName = "your-username",
    
    [Optional]
    Password = "your-password",

    [Required]
    Properties = "name;mail;objectSid;whenCreated",

    if groupSearch ? [Required] : [Optional]
    GroupProperties = "name;objectGuid;objectSid;whenCreated",

    [Optional]
    Filter = "(sAMAccountName=canowar)",

    [Optional]
    Skip = 0,
    
    [Optional]
    Take = 100
};

SearchAsync -> Sadece kullanıcıları sorgular
SearchForGroupsAsync -> Kullanıcılar ile birlikte nested grupları sorgular

Örnek ekran görüntüsü;

![image](https://github.com/user-attachments/assets/d3c35563-a929-405d-835a-5ef4d0ac908a)
