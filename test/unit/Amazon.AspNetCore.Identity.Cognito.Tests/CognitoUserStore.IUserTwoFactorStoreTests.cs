﻿/*
 * Copyright 2019 Amazon.com, Inc. or its affiliates. All Rights Reserved.
 * 
 * Licensed under the Apache License, Version 2.0 (the "License").
 * You may not use this file except in compliance with the License.
 * A copy of the License is located at
 * 
 *  http://aws.amazon.com/apache2.0
 * 
 * or in the "license" file accompanying this file. This file is distributed
 * on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either
 * express or implied. See the License for the specific language governing
 * permissions and limitations under the License.
 */

using Amazon.CognitoIdentityProvider.Model;
using Moq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Amazon.AspNetCore.Identity.Cognito.Tests
{
    public partial class CognitoUserStoreTests
    {
        [Fact]
        public async void Test_GivenAUser_WhenGetTwoFactorEnabledWithAnMFAOption_ThenTheResponseIsTrue()
        {
            var response = new AdminGetUserResponse()
            {
                MFAOptions = new List<MFAOptionType>()
                {
                    new MFAOptionType()
                },
                PreferredMfaSetting = "Setting1",
                UserMFASettingList = new List<string> { "Setting1 "}
            };
            _cognitoClientMock.Setup(mock => mock.AdminGetUserAsync(It.IsAny<AdminGetUserRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(response)).Verifiable();
            var output = await _store.GetTwoFactorEnabledAsync(_userMock.Object, CancellationToken.None).ConfigureAwait(false);
            Assert.True(output);
            _cognitoClientMock.Verify();
        }

        [Fact]
        public async void Test_GivenAUser_WhenGetTwoFactorEnabledWithoutAnMFAOption_ThenTheResponseIsFalse()
        {
            var response = new AdminGetUserResponse()
            {
                MFAOptions = new List<MFAOptionType>()
            };
            _cognitoClientMock.Setup(mock => mock.AdminGetUserAsync(It.IsAny<AdminGetUserRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(response)).Verifiable();
            var output = await _store.GetTwoFactorEnabledAsync(_userMock.Object, CancellationToken.None).ConfigureAwait(false);
            Assert.False(output);
            _cognitoClientMock.Verify();
        }

        [Fact]
        public async void Test_GivenAUser_WhenSetTwoFactorEnabled_ThenAdminSetUserMFAPreferenceAsyncIsCalled()
        {
            _cognitoClientMock.Setup(mock => mock.AdminSetUserMFAPreferenceAsync(It.IsAny<AdminSetUserMFAPreferenceRequest>(), It.IsAny<CancellationToken>())).Returns(Task.FromResult(new AdminSetUserMFAPreferenceResponse())).Verifiable();
            await _store.SetTwoFactorEnabledAsync(_userMock.Object, false, CancellationToken.None).ConfigureAwait(false);
            _cognitoClientMock.Verify();
        }
    }
}
