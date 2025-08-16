using Prototype.Contracts;
using Prototype.Gateway.Services;

var builder = WebApplication.CreateBuilder(args);

// ASP.NET Core API 컨트롤러 서비스를 추가합니다.
builder.Services.AddControllers();

// Add HttpClient for SteamApiService
builder.Services.AddHttpClient<SteamApiService>();

// 인증 서비스를 등록합니다.
builder.Services.AddSingleton<IGuestAuthService, GuestAuthService>();
builder.Services.AddSingleton<ISteamAuthService, SteamAuthService>();
builder.Services.AddSingleton<UserAuthService>();

// Orleans 클라이언트를 설정합니다.
builder.UseOrleansClient(client =>
{
    client.UseLocalhostClustering();
});

var app = builder.Build();

// 웹소켓 미들웨어를 활성화합니다.
app.UseWebSockets();

// HTTP 요청을 컨트롤러에 매핑합니다.
app.MapControllers();

// 테스트용 기본 HTTP GET 엔드포인트를 추가합니다.
app.MapGet("/", () => "Gateway is running.");

await app.RunAsync();