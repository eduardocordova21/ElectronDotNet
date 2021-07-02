# ElectronDotNet

Aplicativo Asp.Net Core + Angular, com o objetivo de transformar todo o site em um aplicativo desktop.

Para tal artimanha, foi utilizado o Electron.NET.

Pré-requisitos:

- .NET 5
- Node 14

## Como foi criado o projeto:

- Criado um aplicativo web Asp.Net Core + Angular, com o template disponível no próprio Visual Studio
- Através do Nuget, foi instalado o pacote ElectronNET.API
- Foi ativada a extensão *UseElectron* na classe *Program.cs*, no método *CreateHostBuild*:

```C#
public static IHostBuilder CreateHostBuilder(string[] args) =>
    Host.CreateDefaultBuilder(args)
        .ConfigureWebHostDefaults(webBuilder =>
        {
----------> webBuilder.UseElectron(args);
            webBuilder.UseStartup<Startup>();
        });
        
```

- Criado o método de *ElectronStartup* na class *Startup.cs*:

```C#
private async void ElectronStartup()
{
    var window = await Electron.WindowManager.CreateWindowAsync();  
    window.OnClosed += () => {  
        Electron.App.Quit();  
    };  
}
```

- Que será chamado no método *Configure* ainda na classe *Startup.cs*:

```C#
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    //...
    
    if (HybridSupport.IsElectronActive)
    {
        ElectronStartup();
    }
}
```

## Como iniciar o projeto:

- Clonar o repositório
- Realizar o build da solução
- Com o CMD, navegar até a pasta do projeto e executar os seguintes comandos:
  - electronize init
  - electronize start
- Voilá, na teoria é para a janela do aplicativo ser mostrada.

## Gerar o instalador NSIS:

- Com o CMD, navegar até a pasta do projeto e executar algum dos seguintes comandos de acordo coma plataforma desejada:
  - electronize build /target win
  - electronize build /target osx
  - electronize build /target linux
- Após a execução do comando, o instalador será gerado no caminho "ElectronDotNet\bin\Desktop"
- Obs.: Para a criação dos instaladores para OSX e Linux, é necessário que o build seja dentro de uma plataforma UNIX

