export type Analysis = {
  id: string;
  name: string;
  messages: Message[];
  created_at: string;
  updated_at: string;
};

export type Message = {
  id: string;
  content: string;
  type: 0 | 1; // 0: Received, 1: Sent
  created_at: string;
  updated_at: string;
};

export type AnalysisPreview = {
  id: string;
  name: string;
  created_at: string;
  updated_at: string;
};
