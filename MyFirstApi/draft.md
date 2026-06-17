else if (verifyPassword == PasswordVerificationResult.SuccessRehashNeeded)
                {
                    UserDto user = new();
                    user.Email = existingUser.Email;
                    user.Id = existingUser.Id;
                    user.Name = existingUser.Name;
                    var token = GetJwtToken(user);

                    existingUser.Password = PasswordHashing(dto);
                    _context.AccountUsers.Update(existingUser);
                    _context.SaveChanges();

                    tokenDto.Token = token;
                    tokenDto.Message = "Login Successful, New Hash Generated";

                    return new Tuple<int, TokenDto>(2, tokenDto);
                }

                else if (verifyPassword == PasswordVerificationResult.Failed)
                {
                    tokenDto.Message = "Password Is Incorrect";
                    return new Tuple<int, TokenDto>(1, tokenDto);
                }
