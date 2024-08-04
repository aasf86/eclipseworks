using eclipseworks.Business.Contracts.UseCases.Taske;
using eclipseworks.Business.Dtos;
using eclipseworks.Business.Dtos.Taske;
using eclipseworks.Domain.Contracts.Repositories.Taske;
using eclipseworks.Infrastructure.EntitiesModels;
using Microsoft.Extensions.Logging;
using System.Data;
using static eclipseworks.Domain.Entities.TaskeComment;

namespace eclipseworks.Business.UseCases.Taske
{
    public class TaskeCommentUseCase : UseCaseBase, ITaskeCommentUseCase
    {
        private readonly ITaskeCommentRepository<TaskeCommentModel> _taskeCommentRepository;
        private ITaskeCommentRepository<TaskeCommentModel> TaskeCommentRepository => _taskeCommentRepository;

        private readonly ITaskeRepository<TaskeModel> _taskeRepository;
        private ITaskeRepository<TaskeModel> TaskeRepository => _taskeRepository;

        public TaskeCommentUseCase(
            ILogger<TaskeCommentUseCase> logger,
            ITaskeCommentRepository<TaskeCommentModel> taskeCommentRepository,
            ITaskeRepository<TaskeModel> taskeRepository,
            IDbConnection dbConnection) : base(logger, dbConnection)
        {
            _taskeCommentRepository = taskeCommentRepository;
            _taskeRepository = taskeRepository;
            TransactionAssigner.Add(TaskeCommentRepository.SetTransaction);
            TransactionAssigner.Add(TaskeRepository.SetTransaction);
        }

        public async Task<ResponseBase<long>> Insert(RequestBase<TaskeCommentInsert> commentInsertRequest)
        {
            try
            {
                var commentInsert = commentInsertRequest.Data;
                var commentInsertResponse = ResponseBase.New((long)0, commentInsertRequest.RequestId);
                var result = Validate(commentInsert);

                if (!result.IsSuccess)
                {
                    commentInsertResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", commentInsertResponse.Errors.ToArray());
                    return commentInsertResponse;
                }

                if (string.IsNullOrEmpty(commentInsert.UserOwner))
                {
                    commentInsertResponse.Errors.Add(TaskeCommentMsgDialog.RequiredUser);
                    return commentInsertResponse;
                }
                
                var commentEntity = new TaskeCommentModel(commentInsert.Comment, commentInsert.UserOwner, long.Parse(commentInsert.TaskeId));
                
                await UnitOfWorkExecute(async () =>
                {
                    var taskeFromDb = await TaskeRepository.GetById(commentEntity.TaskeId);

                    if (taskeFromDb is null)
                    {
                        commentInsertResponse.Errors.Add(TaskeCommentMsgDialog.InvalidTaskeId);
                        return;
                    }

                    await TaskeCommentRepository.Insert(commentEntity);
                    commentInsertResponse.Data = commentEntity.Id;
                });

                return commentInsertResponse;
            }
            catch (Exception exc)
            {
                "Erro no [Insert] comentário: {Comment}".LogErr(commentInsertRequest.Data);
                exc.Message.LogErr(exc);

                var commentInsertResponse = ResponseBase.New((long)0, commentInsertRequest.RequestId);
#if DEBUG
                commentInsertResponse.Errors.Add(exc.Message);
#endif
                commentInsertResponse.Errors.Add("Erro ao inserir comentário.");

                return commentInsertResponse;
            }
        }

        public async Task<ResponseBase<TaskeCommentGet>> GetById(RequestBase<long> commentGetRequest)
        {
            try
            {
                var commentId = commentGetRequest.Data;
                var commentGetResponse = ResponseBase.New(new TaskeCommentGet(), commentGetRequest.RequestId);

                if (commentId <= 0) return commentGetResponse;

                await UnitOfWorkExecute(async () =>
                {
                    var commentFromDb = await TaskeCommentRepository.GetById(commentId);

                    if (commentFromDb is null)
                    {
                        commentGetResponse.Errors.Add(TaskeCommentMsgDialog.NotFound);
                        return;
                    }

                    commentGetResponse.Data = new TaskeCommentGet
                    {
                        Id = commentFromDb.Id,
                        Comment = commentFromDb.Comment,
                        UserOwner = commentFromDb.UserOwner
                    };                    
                });

                return commentGetResponse;
            }
            catch (Exception exc)
            {
                "Erro no [GetById] comentário: {CommentId}".LogErr(commentGetRequest.Data);
                exc.Message.LogErr(exc);

                var commentGetResponse = ResponseBase.New(new TaskeCommentGet(), commentGetRequest.RequestId);
#if DEBUG
                commentGetResponse.Errors.Add(exc.Message);
#endif
                commentGetResponse.Errors.Add("Erro ao obter comentário");

                return commentGetResponse;
            }
        }

        public async Task<ResponseBase<bool>> Update(RequestBase<TaskeCommentUpdate> commentUpdateRequest)
        {
            try
            {
                var commentUpdate = commentUpdateRequest.Data;
                var commentUpdateResponse = ResponseBase.New(false, commentUpdateRequest.RequestId);
                var result = Validate(commentUpdate);

                if (!result.IsSuccess)
                {
                    commentUpdateResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    var errors = string.Join("\n", commentUpdateResponse.Errors.ToArray());
                    return commentUpdateResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var commentFromDb = await TaskeCommentRepository.GetById(long.Parse(commentUpdate.Id));

                    if (commentFromDb is null)
                    {
                        commentUpdateResponse.Errors.Add(TaskeCommentMsgDialog.NotFound);
                        return;
                    }

                    commentFromDb.SetComment(commentUpdate.Comment);
                    commentFromDb.SetLastEventByUser(commentUpdate.UserEvent);

                    await TaskeCommentRepository.Update(commentFromDb);

                    commentUpdateResponse.Data = true;
                });

                return commentUpdateResponse;
            }
            catch (Exception exc)
            {
                "Erro ao [Update] comentário: {CommentId}".LogErr(commentUpdateRequest.Data.Id);
                exc.Message.LogErr(exc);

                var commentUpdateResponse = ResponseBase.New(false, commentUpdateRequest.RequestId);
#if DEBUG
                commentUpdateResponse.Errors.Add(exc.Message);
#endif
                commentUpdateResponse.Errors.Add("Erro ao alterar comentário.");

                return commentUpdateResponse;
            }
        }

        public async Task<ResponseBase<bool>> Delete(RequestBase<TaskeCommentDelete> commentDeleteRequest)
        {
            try
            {
                var commentDelete = commentDeleteRequest.Data;
                var commentDeleteResponse = ResponseBase.New(false, commentDeleteRequest.RequestId);
                var result = Validate(commentDelete);

                if (!result.IsSuccess)
                {
                    commentDeleteResponse.Errors.AddRange(result.Validation.Select(x => x.ErrorMessage).ToList());
                    return commentDeleteResponse;
                }

                await UnitOfWorkExecute(async () =>
                {
                    var commentFromDb = await TaskeCommentRepository.GetById(long.Parse(commentDelete.Id));

                    if (commentFromDb is null)
                    {
                        commentDeleteResponse.Errors.Add(TaskeCommentMsgDialog.NotFound);
                        return;
                    }

                    commentFromDb.SetLastEventByUser(commentDelete.UserEvent);

                    await TaskeCommentRepository.Update(commentFromDb);

                    await TaskeCommentRepository.Delete(commentFromDb);

                    commentDeleteResponse.Data = true;
                });

                return commentDeleteResponse;
            }
            catch (Exception exc)
            {
                "Erro [Delete] comentário: {CommentId}".LogErr(commentDeleteRequest.Data.Id);
                exc.Message.LogErr(exc);

                var commentDeleteResponse = ResponseBase.New(false, commentDeleteRequest.RequestId);
#if DEBUG
                commentDeleteResponse.Errors.Add(exc.Message);
#endif
                commentDeleteResponse.Errors.Add("Erro ao excluir comentário.");

                return commentDeleteResponse;
            }
        }

        public async Task<ResponseBase<List<TaskeCommentGet>>> GetAll(RequestBase<string> commentGetRequest)
        {
            try
            {
                var commentGetResponse = ResponseBase.New(new List<TaskeCommentGet>(), commentGetRequest.RequestId);

                await UnitOfWorkExecute(async () =>
                {
                    var listCommentsFromDb = await TaskeCommentRepository.GetAll(commentGetRequest.Data);

                    commentGetResponse.Data = listCommentsFromDb.Select(x => new TaskeCommentGet
                    {
                        Id = x.Id,
                        Comment = x.Comment,
                        UserOwner = x.UserOwner
                    }).ToList();
                });

                return commentGetResponse;
            }
            catch (Exception exc)
            {
                "Erro no [GetAll] comentários: {Comment}".LogErr(commentGetRequest.Data);
                exc.Message.LogErr(exc);

                var commentGetResponse = ResponseBase.New(new List<TaskeCommentGet>(), commentGetRequest.RequestId);
#if DEBUG
                commentGetResponse.Errors.Add(exc.Message);
#endif
                commentGetResponse.Errors.Add("Erro ao obter comentários");

                return commentGetResponse;
            }
        }

    }
}
