using AuctionApp.Data.Repositories.Interfaces;
using AuctionApp.Domain.DataTransferObject.User;
using AuctionApp.Domain.Entities.User;
using AuctionApp.Service.Helpers;
using AuctionApp.Service.Interfaces;
using AutoMapper;

namespace AuctionApp.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;

        public UserService(
            IUserRepository userRepository,
            ILogger<UserService> logger,
            IMapper mapper)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<Result<UserDTO>> CreateAccount(UserDTO userDTO)
        {
            Result<UserDTO> result = new(false);

            try
            {
                userDTO.UserCode = new RandomGenerator().GenerateRandomCode(5);

                var user = _mapper.Map<User>(userDTO);
                var response = await _userRepository.CreateUser(user);
                if (response == null)
                {

                    result.SetError("Email already has an account created", "Try a different email");
                }
                else
                {
                    result.SetSuccess(_mapper.Map<UserDTO>(response), "Account Created Successfully");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while creating Account");
                result.SetError(ex.ToString(), "Error while creating Account");
            }
            return result;
        }

        public async Task<Result<List<GetUserDTO>>> GetBuyers()
        {
            Result<List<GetUserDTO>> result = new(false);

            try
            {
                var response = await _userRepository.GetAllBuyers();

                if (response.Count == 0)
                {
                    result.SetError("Error retrieving buyers", "No buyer has been registered");
                }
                else
                {
                    var buyer = _mapper.Map<List<GetUserDTO>>(response);
                    result.SetSuccess(buyer, "Retrieved Successfully");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving User ");
                result.SetError(e.ToString(), "No buyer has been registered");
            }

            return result;
        }
        public async Task<Result<List<GetUserDTO>>> GetSellers()
        {
            Result<List<GetUserDTO>> result = new(false);

            try
            {
                var response = await _userRepository.GetAllSellers();

                if (response.Count == 0)
                {
                    result.SetError("Error retrieving sellers", "No seller has been registered");
                }
                else
                {
                    var buyer = _mapper.Map<List<GetUserDTO>>(response);
                    result.SetSuccess(buyer, "Retrieved Successfully");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving User ");
                result.SetError(e.ToString(), "No seller has been registered");
            }

            return result;
        }

        public async Task<Result<GetUserDTO>> GetUserByCode(string userCode)
        {
            Result<GetUserDTO> result = new(false);

            try
            {
                var response = await _userRepository.GetUserByCode(userCode);

                if (response == null)
                {
                    result.SetError("Error retrieving user", "User does not exist");
                }
                else
                {
                    result.SetSuccess(_mapper.Map<GetUserDTO>(response), "User retrived successfully");
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving User");
                result.SetError(e.ToString(), "Users does not exist");
            }

            return result;
        }


        public async Task<Result<List<GetUserDTO>>> GetUsers()
        {
            Result<List<GetUserDTO>> result = new(false);

            try
            {
                var response = await _userRepository.GetAllUsers();

                if (response.Count == 0)
                {
                    result.SetError("Error retrieving user", "User does not exist");
                }
                else
                {
                    var user = _mapper.Map<List<GetUserDTO>>(response);
                    result.SetSuccess(user, "Retrieved Successfully");
                }
            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving User ");
                result.SetError(e.ToString(), "Users does not exist");
            }

            return result;
        }

        public async Task<Result<List<GetUserDTO>>> GetUserByUserName(string Username)
        {
            Result<List<GetUserDTO>> result = new(false);

            try
            {
                var response = await _userRepository.GetUserByName(Username);
                if (response.Count == 0)
                {
                    result.SetError("Error retrieving user", "User does not exist");
                }
                else
                {
                    var user = _mapper.Map<List<GetUserDTO>>(response);

                    result.SetSuccess(user, "Retrieved Successfully");
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, "Error retrieving User ");
                result.SetError(e.ToString(), "Users does not exist");
            }

            return result;
        }
   
        public async Task<Result<bool>> DeleteUserAccount(string userCode)
        {
            Result<bool> result = new(false);

            try
            {
                var response = await _userRepository.DeleteUserAccount(userCode);
                result.Content = response;

                if (!response)
                {
                    result.SetError("User not found", $"User with Id {userCode} does not exist");
                }
                else
                {
                    result.SetSuccess(response, $"User with Id {userCode} deleted Successfully !");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while removing User");
                result.SetError(ex.ToString(), "Error while removing User");
            }

            return result;
        }

    }
}
