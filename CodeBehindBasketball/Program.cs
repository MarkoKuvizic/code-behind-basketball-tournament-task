using CodeBehindBasketball.model;
using CodeBehindBasketball.repo;
using CodeBehindBasketball.service;


IGroupRepository groupRepository = new GroupJsonRepository("../../../../groups.json");
IScheduleService scheduleService = new SimpleScheduleService(groupRepository, new FibaRankingFormOddsStrategy(), new WeighedScoreGameSimulationStrategy(), new BasicGroupRankingStrategy());
scheduleService.GenerateGroupStageSchedule();
scheduleService.ExecuteAllRounds();
scheduleService.PrintSchedule();
scheduleService.ExecuteKnockoutStage();