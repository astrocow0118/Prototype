using Xunit;
using Moq;
using FluentAssertions;
using Prototype.Contracts;
using Prototype.Gateway.Services;
using Orleans;
using System;
using System.Threading.Tasks;

namespace Prototype.Tests;

public class UserAuthServiceTests
{
    private readonly Mock<IClusterClient> _mockClusterClient;
    private readonly Mock<IGuestAuthService> _mockGuestAuthService;
    private readonly Mock<ISteamAuthService> _mockSteamAuthService;
    private readonly UserAuthService _userAuthService;

    public UserAuthServiceTests()
    {
        // Common Arrange: Setup mocks for all tests in this class
        _mockClusterClient = new Mock<IClusterClient>();
        _mockGuestAuthService = new Mock<IGuestAuthService>();
        _mockSteamAuthService = new Mock<ISteamAuthService>();

        // Instantiate the service with mocked dependencies
        _userAuthService = new UserAuthService(
            _mockClusterClient.Object,
            _mockGuestAuthService.Object,
            _mockSteamAuthService.Object
        );
    }

    [Fact]
    public async Task AuthenticateAsync_WithGuestProvider_ShouldReturnGuestAccount()
    {
        // Arrange
        var provider = AuthProvider.Guest;
        var token = "test_guest_token";
        var expectedAccountKey = $"guest:{token}";
        var expectedAccount = new Account(expectedAccountKey, Guid.NewGuid(), DateTime.UtcNow);

        var mockAccountGrain = new Mock<IAccountGrain>();

        // Setup mock behaviors
        _mockGuestAuthService.Setup(s => s.Authenticate(token))
            .ReturnsAsync(expectedAccountKey);

        _mockClusterClient.Setup(c => c.GetGrain<IAccountGrain>(expectedAccountKey, null))
            .Returns(mockAccountGrain.Object);

        mockAccountGrain.Setup(g => g.Authenticate())
            .ReturnsAsync(expectedAccount);

        // Act
        var result = await _userAuthService.AuthenticateAsync(provider, token);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(expectedAccount); // Compare object properties

        // Verify that dependencies were called
        _mockGuestAuthService.Verify(s => s.Authenticate(token), Times.Once);
        _mockClusterClient.Verify(c => c.GetGrain<IAccountGrain>(expectedAccountKey, null), Times.Once);
        mockAccountGrain.Verify(g => g.Authenticate(), Times.Once);
    }
}