using Advertising_Agency.BusinessLogic.Services;
using AutoFixture.AutoNSubstitute;
using AutoFixture;
using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Advertising_Agency.Tests.ServisesTests
{
    public class BCryptPasswordHasherTests
    {
        private readonly IFixture _fixture;
        private readonly BCryptPasswordHasher _hasher;

        public BCryptPasswordHasherTests()
        {
            _fixture = new Fixture().Customize(new AutoNSubstituteCustomization());

            // Уникнення циклів, якщо будуть складні об'єкти
            _fixture.Behaviors
                .OfType<ThrowingRecursionBehavior>()
                .ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _hasher = new BCryptPasswordHasher();
        }

        [Fact]
        public void HashPassword_ShouldReturnDifferentHash_ForSamePassword()
        {
            // Arrange
            var password = _fixture.Create<string>();

            // Act
            var hash1 = _hasher.HashPassword(password);
            var hash2 = _hasher.HashPassword(password);

            // Assert
            hash1.Should().NotBe(hash2);
        }

        [Fact]
        public void VerifyPassword_ShouldReturnTrue_WhenPasswordMatchesHash()
        {
            // Arrange
            var password = _fixture.Create<string>();
            var hash = _hasher.HashPassword(password);

            // Act
            var result = _hasher.VerifyPassword(hash, password);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void VerifyPassword_ShouldReturnFalse_WhenPasswordDoesNotMatchHash()
        {
            // Arrange
            var password = _fixture.Create<string>();
            var wrongPassword = _fixture.Create<string>();
            var hash = _hasher.HashPassword(password);

            // Act
            var result = _hasher.VerifyPassword(hash, wrongPassword);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void HashPassword_ShouldNotReturnNullOrEmpty()
        {
            // Arrange
            var password = _fixture.Create<string>();

            // Act
            var hash = _hasher.HashPassword(password);

            // Assert
            hash.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public void VerifyPassword_ShouldThrowException_WhenHashIsInvalid()
        {
            // Arrange
            var password = _fixture.Create<string>();
            var invalidHash = "invalid_hash";

            // Act
            Action act = () => _hasher.VerifyPassword(invalidHash, password);

            // Assert
            act.Should().Throw<Exception>();
        }
    }
}
