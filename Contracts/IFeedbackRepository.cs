using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts
{
    public interface IFeedbackRepository
    {
        void CreateFeedback(Feedback order);
        void DeleteFeedback(Feedback order);
        Task<IEnumerable<Feedback>> GetAllFeedbacksAsync(bool trackChanges);
        //Task<IEnumerable<Feedback>> GetFeedbacksByAccountAsync(Guid userId, bool trackChanges);//получение всех комментариев по аккаунту

    }
}
